import { Component, Input } from '@angular/core';
import { Category } from '../../category.model';
import { CategoryService } from '../../category.service';

@Component({
  selector: 'app-category-item',
  templateUrl: './category-item.component.html',
  styleUrls: ['./category-item.component.css']
})
export class CategoryItemComponent {
  @Input() category: Category;

  constructor(private categoryService: CategoryService) { }

  onRemove(id: string) {
    this.categoryService.deleteCategory(id).subscribe();
  }
}
