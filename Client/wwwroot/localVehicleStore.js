(function () {
    // This code exists to support functionality in LocalVehicleStore.cs. It provides convenient access to
    // the browser's IndexedDB APIs, along with a preconfigured database structure.

    const db = idb.openDB('Vehicles', 1, {
        upgrade(db) {
            db.createObjectStore('metadata');
            db.createObjectStore('serverdata', { keyPath: 'licenseNumber' }).createIndex('lastUpdated', 'lastUpdated');
            db.createObjectStore('localedits', { keyPath: 'licenseNumber' });
        },
    });

    window.localVehicleStore = {
        get: async (storeName, key) => (await db).transaction(storeName).store.get(key),
        getAll: async (storeName) => (await db).transaction(storeName).store.getAll(),
        getFirstFromIndex: async (storeName, indexName, direction) => {
            const cursor = await (await db).transaction(storeName).store.index(indexName).openCursor(null, direction);
            return (cursor && cursor.value) || null;
        },
        put: async (storeName, key, value) => (await db).transaction(storeName, 'readwrite').store.put(value, key === null ? undefined : key),
        putAllFromJson: async (storeName, json) => {
            const store = (await db).transaction(storeName, 'readwrite').store;
            JSON.parse(json).forEach(item => store.put(item));
        },
        delete: async (storeName, key) => (await db).transaction(storeName, 'readwrite').store.delete(key),
        autocompleteKeys: async (storeName, text, maxResults) => {
            const results = [];
            let cursor = await (await db).transaction(storeName).store.openCursor(IDBKeyRange.bound(text, text + '\uffff'));
            while (cursor && results.length < maxResults) {
                results.push(cursor.key);
                cursor = await cursor.continue();
            }
            return results;
        }
    };
})();
