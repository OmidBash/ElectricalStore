export class User {
    constructor(
        public email: string,
        public userId: string,
        private _token: string,
        private _tokenexpirationDate: Date) { }

    get token() {
        if (!this._tokenexpirationDate || this._tokenexpirationDate < new Date()) {
            return null;
        } else {
            return this._token;
        }
    }

    get ExpirationDuration() {
        return new Date(this._tokenexpirationDate).getTime() - new Date().getTime();
    }
}
