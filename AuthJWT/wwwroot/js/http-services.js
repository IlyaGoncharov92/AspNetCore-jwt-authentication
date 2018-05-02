const key = 'token';

class Storage {
    static get() {
        var item = localStorage.getItem(key);
        return JSON.parse(item);
    }

    static set(data) {
        localStorage.setItem(key, JSON.stringify(data));
    }

    static remove() {
        localStorage.removeItem(key);
    }
}

class Http {
    get headers() {
        var headers;

        var auth = Storage.get();

        if (auth) {
            headers = {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${auth.access_token}`
            };
        }
        else {
            headers = {
                'Content-Type': 'application/json'
            };
        }

        return headers;
    }

    get(url) {
        var options = {
            method: 'GET',
            headers: this.headers
        };
        return fetch(url, options);
    }

    post(url, data) {
        var options = {
            method: 'POST',
            headers: this.headers,
            body: JSON.stringify(data)
        };
        return fetch(url, options);
    }
}
