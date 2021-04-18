import { FeatureType } from './feature-type.model';

export class Feature {
    feature: any;
    constructor
        (
            public id?: string,
            public title?: string,
            public price?: number,
            public description?: string,
            public featureType?: FeatureType
        ) { }
}
