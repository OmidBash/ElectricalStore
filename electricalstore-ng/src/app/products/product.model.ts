import { Category } from '../categories/category.model';
import { Feature } from '../features/feature.model';
import { Image } from '../shared/image.model';

export class Product {
    constructor(
            public id?: string,
            public title?: string,
            public description?: string,
            public price?: number,
            public imagePaths?: Image[],
            public categories?: Category[],
            public features?: Feature[]
        ) { }
}
