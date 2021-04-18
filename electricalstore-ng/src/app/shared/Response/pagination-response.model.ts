export class PaginationResponse {
    constructor
        (
            public data?: any,
            public pageNumber?: string,
            public pageSize?: string,
            public length?: number
        ) { }
}