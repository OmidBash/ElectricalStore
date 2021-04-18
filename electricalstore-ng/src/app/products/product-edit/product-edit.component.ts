import { HttpEventType } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { MatCheckboxChange, MatSlideToggleChange } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { Category } from 'src/app/categories/category.model';
import { AppConfig } from 'src/app/config/config';
import { FeatureType } from 'src/app/features/feature-type.model';
import { FeatureTypeService } from 'src/app/features/feature-types.service';
import { Feature } from 'src/app/features/feature.model';
import { FileInformation, FileManagerService } from 'src/app/shared/fileManager.service';
import { Product } from '../product.model';
import { ProductsService } from '../products.service';
import { IProductEditor } from './product-edit-resolver.service';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.css'],
  host: { class: 'editor' }
})
export class ProductEditComponent implements OnInit {

  get categoryControls() {
    if (this.productForm.controls.categories) {
      return (this.productForm.controls.categories as FormArray).controls;
    } else { return null; }
  }

  get featureControls() {
    if (this.productForm.controls.features) {
      return (this.productForm.controls.features as FormArray).controls;
    } else { return null; }
  }

  get imageControls() {
    if (this.productForm.controls.imagePaths) {
      return (this.productForm.controls.imagePaths as FormArray).controls;
    } else { return null; }
  }

  constructor(
    private productService: ProductsService,
    private featureTypeService: FeatureTypeService,
    private fileManagerService: FileManagerService,
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private appConfig: AppConfig
  ) { }

  product = new Product();
  productForm: FormGroup;
  editMode = false;
  progress: number;
  totalPrice: number;

  categories: Category[];
  featureTypes: FeatureType[];

  formData = new FormData();
  hasNewImage: boolean;

  ngOnInit() {
    this.route.data.subscribe((productEditor: IProductEditor) => {
      this.product = productEditor[0].product;
      this.categories = productEditor[0].categories;
      this.featureTypes = productEditor[0].featureTypes;
      this.editMode = this.product.id !== undefined;
      this.initForm();
      if (this.editMode) {
        this.fillFormData();
      }
    });
  }

  initForm() {
    this.productForm = this.fb.group({
      id: new FormControl(''),
      title: new FormControl(''),
      description: new FormControl(''),
      price: new FormControl(''),
      categories: this.fb.array([]),
      features: this.fb.array([]),
      imagePaths: this.fb.array([])
    });

    const categoryArray = (this.productForm.get('categories') as FormArray);
    categoryArray.clear();
    this.categories.forEach(category => {
      categoryArray.push(this.addCategoryFormGroup(category));
    });

    (this.productForm.controls.features as FormArray).valueChanges.subscribe(() =>
      this.calculateTotalPrice()
    );

    (this.productForm.controls.price).valueChanges.subscribe(() =>
      this.calculateTotalPrice()
    );
  }

  fillFormData() {
    this.productForm.patchValue(
      {
        id: this.product.id,
        title: this.product.title,
        description: this.product.description,
        price: this.product.price
      }
    );

    this.product.imagePaths.forEach((imgPath) => {
      (this.productForm.controls.imagePaths as FormArray)
        .push(this.addImageFormGroup(imgPath.id, imgPath.file, imgPath.url, imgPath.caption));
    });

    if (this.product.features) {
      const productFeatureTypes = this.featureTypes.filter(ft =>
        this.product.features.some(pf =>
          pf.feature.featureType.id === ft.id));
      productFeatureTypes.forEach(featureType => {
        this.getFeatures(featureType.id);
      });
    }
  }

  getFeatures(featureTypeId: string) {
    this.featureTypeService.getFeatureTypeDetailById(featureTypeId).subscribe(featureType => {
      const featureArray = (this.productForm.get('features') as FormArray);
      featureType.features.forEach(feature => {
        feature.featureType = featureType;
        featureArray.push(this.addFeatureFormGroup(feature));
      });
    },
      error => {
        console.log(error);
      }
    );
  }

  hasFeatureType(featureTypeId: string): boolean {
    let hasFeatureType = false;
    if (this.product.features) {
      hasFeatureType = this.product.features.some(f => f.feature.featureType.id === featureTypeId);
    }
    return hasFeatureType;
  }

  hasFeature(featureId: string): boolean {
    let hasFeature = false;
    if (this.product.features) {
      hasFeature = this.product.features.findIndex(f => f.feature.id === featureId) > -1;
    }
    return hasFeature;
  }

  addCategoryFormGroup(category: Category) {
    let hasCategory = false;
    if (this.product.categories) {
      hasCategory = this.product.categories.findIndex(cat => cat.category.id === category.id) > -1;
    }

    if (hasCategory && this.editMode) {
      return this.fb.group({
        categoryId: new FormControl(category.id),
        productId: new FormControl(this.product.id),
        title: new FormControl(category.title),
        selected: new FormControl(true),
      });
    } else {
      return this.fb.group({
        categoryId: new FormControl(category.id),
        title: new FormControl(category.title),
        selected: new FormControl(false),
      });
    }
  }

  addFeatureFormGroup(feature: Feature) {
    if (this.hasFeature(feature.id) && this.editMode) {
      return this.fb.group({
        featureTypeId: new FormControl(feature.featureType.id),
        featureId: new FormControl(feature.id),
        productId: new FormControl(this.product.id),
        title: new FormControl(feature.title),
        price: new FormControl(this.product.features.find(f => f.feature.id === feature.id).price),
        selected: new FormControl(true),
      });
    } else {
      return this.fb.group({
        featureTypeId: new FormControl(feature.featureType.id),
        featureId: new FormControl(feature.id),
        title: new FormControl(feature.title),
        price: new FormControl(0),
        selected: new FormControl(false),
      });
    }
  }

  addImageFormGroup(id: string, imgData: any, url: string, caption: string): FormGroup {
    if (this.editMode
      && this.product.imagePaths.findIndex(img => img.id === id) > -1) {
      return this.fb.group({
        id,
        productId: this.product.id,
        image: imgData === undefined ? this.appConfig.setting['PathHost'] + url : imgData,
        url,
        caption: new FormControl(caption, Validators.required)
      });
    } else {
      return this.fb.group({
        image: imgData,
        url,
        caption: new FormControl(caption, Validators.required)
      });
    }
  }

  onChangeCategoryToggle(event: MatSlideToggleChange, index: number) {
    const categoryArrayForm = this.productForm.get('categories') as FormArray;
    const categoryFormControl = (categoryArrayForm.at(index) as FormGroup).value;

    if (event.checked
      && this.editMode
      && this.product.categories.findIndex(cat => cat.id === categoryFormControl.categoryId) > -1) {
      (categoryArrayForm.at(index) as FormGroup).addControl("productId", new FormControl(this.product.id));
    }
  }

  onChangeFeatureType(event: MatCheckboxChange, featureTypeId: string) {
    if (event.checked) {
      this.getFeatures(featureTypeId);
    } else {
      const featureArrayForm = this.productForm.get('features') as FormArray;

      const featureControls = featureArrayForm.controls.filter(control => {
        return control.value.featureTypeId === featureTypeId;
      });

      featureControls.forEach(featureControl => {
        const index = featureArrayForm.controls.findIndex(control => {
          return control === featureControl;
        });
        featureArrayForm.controls.splice(index, 1);
        featureArrayForm.value.splice(index, 1);
      });
    }
  }

  onChangeFeature(event: MatCheckboxChange, index: number) {
    const featureArrayForm = this.productForm.get('features') as FormArray;
    const featureFormControl = (featureArrayForm.at(index) as FormGroup).value;
    if (event.checked
      && this.editMode
      && this.hasFeature(featureFormControl.featureId)) {
      (featureArrayForm.at(index) as FormGroup).addControl("productId", new FormControl(this.product.id));
    }
  }

  onAddImage(event) {
    const filesToUpload: File[] = event.target.files;
    const reader = new FileReader();

    Array.from(filesToUpload).map((file, index) => {
      this.formData.append(file.name, file, file.name);

      reader.readAsDataURL(file);
      reader.onload = () => {
        (this.productForm.get('imagePaths') as FormArray).push(
          this.addImageFormGroup(null, reader.result, file.name, file.name)
        );
      };

      this.hasNewImage = true;
    });
  }

  onRemoveImage(index: number) {
    (this.productForm.get('imagePaths') as FormArray).removeAt(index);
  }

  removeNotSelectedItemsOfFormArrases() {
    const categoryArrayForm = this.productForm.get('categories') as FormArray;
    const notSelectedCategories = categoryArrayForm.controls.filter(control => {
      return control.value.selected === false;
    });
    notSelectedCategories.forEach(notSelectedCategory => {
      const index = categoryArrayForm.controls.indexOf(notSelectedCategory);
      categoryArrayForm.removeAt(index);
    });

    const featureArrayForm = this.productForm.get('features') as FormArray;
    const notSelectedFeatures = featureArrayForm.controls.filter(control => {
      return control.value.selected === false;
    });
    notSelectedFeatures.forEach(notSelectedFeature => {
      const index = featureArrayForm.controls.indexOf(notSelectedFeature);
      featureArrayForm.removeAt(index);
    });
  }

  onSubmitForm() {
    this.removeNotSelectedItemsOfFormArrases();

    // const obs$ = new Observable();
    // obs$.pipe(
    //   map(() => {
    //     if (this.hasNewImage) {
    //       this.fileManagerService.uploadFile(this.formData).subscribe(event => {
    //         if (event.type === HttpEventType.UploadProgress) {
    //           console.log(Math.round(100 * event.loaded / event.total));
    //           this.progress = Math.round(100 * event.loaded / event.total);
    //         } else if (event.type === HttpEventType.Response) {
    //           this.progress = 0;
    //           return event.body.data;
    //         }
    //       });
    //     } else {
    //       return null;
    //     }
    //   }),
    //   map((paths: FileInformation) => {
    //     if (paths) {
    //       (this.productForm.get('imagePaths') as FormArray).controls.map(imageControl => {
    //         if (paths[imageControl.value.url]) {
    //           imageControl.patchValue({
    //             url: paths[imageControl.value.url]
    //           });
    //         }
    //       });
    //     }

    //     if (this.editMode) {
    //       this.productService.updateProduct(this.productForm.value).subscribe();
    //     } else {
    //       this.productService.addProduct(this.productForm.value).subscribe();
    //     }
    //   })
    // ).subscribe({
    //   next: data => console.log(data),
    //   error: error => console.log(error),
    //   complete: () => this.reset()
    // });

    if (this.hasNewImage) {
      this.fileManagerService.uploadFile(this.formData).subscribe(event => {
        if (event.type === HttpEventType.UploadProgress) {
          console.log(Math.round(100 * event.loaded / event.total));
          this.progress = Math.round(100 * event.loaded / event.total);
        } else
          if (event.type === HttpEventType.Response) {
            this.progress = 0;
            const pathData: FileInformation[] = event.body.data;
            (this.productForm.get('imagePaths') as FormArray).controls.map(imageControl => {
              if (pathData[imageControl.value.url]) {
                imageControl.patchValue({
                  url: pathData[imageControl.value.url]
                });
              }
            });
          }
        if (this.editMode) {
          this.productService.updateProduct(this.productForm.value).subscribe();
        } else {
          this.productService.addProduct(this.productForm.value).subscribe();
        }
      });
    } else {
      if (this.editMode) {
        this.productService.updateProduct(this.productForm.value).subscribe();
      } else {
        this.productService.addProduct(this.productForm.value).subscribe();
      }
    }

    this.reset();
  }

  onCancelForm() {
    this.reset();
  }

  reset() {
    // this.featuretype = new FeatureType();
    // this.editForm.reset();
    // this.addFeatureForm.reset()
    // this.editMode = false;
    this.router.navigate(['../'], { relativeTo: this.route });
  }

  calculateTotalPrice() {
    this.totalPrice = 0;
    this.totalPrice += +this.productForm.value.price;
    (this.productForm.controls.features as FormArray).controls
      .forEach(control => { this.totalPrice += +control.value.price; });
  }
}
