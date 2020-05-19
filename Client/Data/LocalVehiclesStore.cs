using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using CarChecker.Shared;
using Microsoft.JSInterop;

namespace CarChecker.Client.Data
{
    // To support offline use, we use this simple local data repository
    // instead of performing data access directly against the server.
    // This would not be needed if we assumed that network access was always
    // available.

    public class LocalVehiclesStore
    {
        private readonly HttpClient httpClient;
        private readonly IJSRuntime js;

        public LocalVehiclesStore(HttpClient httpClient, IJSRuntime js)
        {
            this.httpClient = httpClient;
            this.js = js;
        }

        public ValueTask<Vehicle[]> GetOutstandingLocalEditsAsync()
        {
            return js.InvokeAsync<Vehicle[]>(
                "localVehicleStore.getAll", "localedits");
        }

        public async Task SynchronizeAsync()
        {
            // If there are local edits, always send them first
            foreach (var editedVehicle in await GetOutstandingLocalEditsAsync())
            {
                (await httpClient.PutAsJsonAsync("api/vehicle/details", editedVehicle)).EnsureSuccessStatusCode();
                await DeleteAsync("localedits", editedVehicle.LicenseNumber);
            }

            await FetchChangesAsync();
        }

        public ValueTask SaveUserAccountAsync(ClaimsPrincipal user)
        {
            return user != null
                ? PutAsync("metadata", "userAccount", user.Claims.Select(c => new ClaimData { Type = c.Type, Value = c.Value }))
                : DeleteAsync("metadata", "userAccount");
        }

        public async Task<ClaimsPrincipal> LoadUserAccountAsync()
        {
            var storedClaims = await GetAsync<ClaimData[]>("metadata", "userAccount");
            return storedClaims != null
                ? new ClaimsPrincipal(new ClaimsIdentity(storedClaims.Select(c => new Claim(c.Type, c.Value)), "appAuth"))
                : new ClaimsPrincipal(new ClaimsIdentity());
        }

        public ValueTask<string[]> Autocomplete(string prefix)
            => js.InvokeAsync<string[]>("localVehicleStore.autocompleteKeys", "serverdata", prefix, 5);

        // If there's an outstanding local edit, use that. If not, use the server data.
        public async Task<Vehicle> GetVehicle(string licenseNumber)
            => await GetAsync<Vehicle>("localedits", licenseNumber)
            ?? await GetAsync<Vehicle>("serverdata", licenseNumber);

        public async ValueTask<DateTime?> GetLastUpdateDateAsync()
        {
            var value = await GetAsync<string>("metadata", "lastUpdateDate");
            return value == null ? (DateTime?)null : DateTime.Parse(value);
        }

        public ValueTask SaveVehicleAsync(Vehicle vehicle)
            => PutAsync("localedits", null, vehicle);

        async Task FetchChangesAsync()
        {
            var mostRecentlyUpdated = await js.InvokeAsync<Vehicle>("localVehicleStore.getFirstFromIndex", "serverdata", "lastUpdated", "prev");
            var since = mostRecentlyUpdated?.LastUpdated ?? DateTime.MinValue;
            var json = await httpClient.GetStringAsync($"api/vehicle/changedvehicles?since={since:o}");
            await js.InvokeVoidAsync("localVehicleStore.putAllFromJson", "serverdata", json);
            await PutAsync("metadata", "lastUpdateDate", DateTime.Now.ToString("o"));
        }

        ValueTask<T> GetAsync<T>(string storeName, object key)
            => js.InvokeAsync<T>("localVehicleStore.get", storeName, key);

        ValueTask PutAsync<T>(string storeName, object key, T value)
            => js.InvokeVoidAsync("localVehicleStore.put", storeName, key, value);

        ValueTask DeleteAsync(string storeName, object key)
            => js.InvokeVoidAsync("localVehicleStore.delete", storeName, key);

        class ClaimData
        {
            public string Type { get; set; }
            public string Value { get; set; }
        }
    }
}
