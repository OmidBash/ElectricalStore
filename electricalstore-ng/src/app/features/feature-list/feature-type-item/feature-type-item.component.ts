import { Component, Input, OnInit, Output } from '@angular/core';
import { Subject } from 'rxjs';
import { FeatureType } from '../../feature-type.model';
import { FeatureTypeService } from '../../feature-types.service';

@Component({
  selector: 'app-feature-type-item',
  templateUrl: './feature-type-item.component.html',
  styleUrls: ['./feature-type-item.component.css']
})
export class FeatureTypeItemComponent {
  @Input() featureType: FeatureType;

  constructor(private featureTypeService: FeatureTypeService) { }

  onRemove(id: string) {
    this.featureTypeService.deleteFeatureType(id).subscribe();
  }
}
