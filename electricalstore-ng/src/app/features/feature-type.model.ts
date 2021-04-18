import { Feature } from './feature.model';

export class FeatureType {
    constructor(
            public id?: string,
            public title?: string,
            public description?: string,
            public features?: Feature[]
        ) { }
}
