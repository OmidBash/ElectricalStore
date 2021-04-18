import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FeatureType } from '../feature-type.model';
import { FeatureTypeService } from '../feature-types.service';

@Component({
  selector: 'app-feature-edit',
  templateUrl: './feature-edit.component.html',
  styleUrls: ['./feature-edit.component.css'],
  host: {
    class: 'editor'
  }
})
export class FeatureEditComponent implements OnInit {
  featureTypeForm: FormGroup;
  featureType = new FeatureType();
  editMode = false;

  get featureControls() {
    if (this.featureTypeForm.controls.features) {
      return (this.featureTypeForm.controls.features as FormArray).controls;
    } else { return null; }
  }

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private featureTypeService: FeatureTypeService
  ) { }

  ngOnInit() {
    this.initFrom();

    this.route.data.subscribe((featureType: FeatureType) => {
      this.featureType = featureType[0];
      this.editMode = this.featureType.id !== undefined;
      this.fillFormData();
    });
  }

  private initFrom() {
    this.featureTypeForm = this.fb.group({
      id: '',
      title: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required),
      features: this.fb.array([])
    });
  }

  fillFormData() {
    this.featureTypeForm.patchValue(
      {
        id: this.featureType.id,
        title: this.featureType.title,
        description: this.featureType.description,
      }
    );

    if (this.featureType.features) {
      this.addFeatureControls();
    }
  }

  addFeatureFormGroup(title: string, description: string): FormGroup {
    return this.fb.group({
      featureTypeId: this.featureType.id,
      title: new FormControl(title, Validators.required),
      description: new FormControl(description, Validators.required)
    });
  }

  addFeatureControls() {
    const featuresArray = this.featureTypeForm.get('features') as FormArray;
    for (const feature of this.featureType.features) {
      featuresArray.push(new FormGroup(
        {
          id: new FormControl(feature.id),
          featureTypeId: new FormControl(this.featureType.id),
          title: new FormControl(feature.title, Validators.required),
          description: new FormControl(feature.description, Validators.required),
        }
      ));
    }
  }

  onAddFeature(addFeatureForm: NgForm) {
    (this.featureTypeForm.get('features') as FormArray).push(
      this.addFeatureFormGroup(addFeatureForm.value.add_title, addFeatureForm.value.add_description)
    );

    addFeatureForm.reset();
  }

  onRemoveFeature(index: number) {
    (this.featureTypeForm.get('features') as FormArray).removeAt(index);
  }

  onSubmitForm() {
    if (this.editMode) {
      this.featureTypeService.updateFeatureType(this.featureTypeForm.value).subscribe();
    } else {
      this.featureTypeService.addFeatureType(this.featureTypeForm.value).subscribe();
    }

    this.reset();
  }

  onCancelForm() {
    this.reset();
  }

  reset() {
    this.featureType = new FeatureType();
    this.featureTypeForm.reset();
    this.router.navigate(['../'], {relativeTo: this.route});
  }
}
