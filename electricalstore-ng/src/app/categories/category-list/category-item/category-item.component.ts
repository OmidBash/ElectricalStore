import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Category } from '../../category.model';
import { CategoryService } from '../../category.service';

@Component({
  selector: 'app-category-item',
  templateUrl: './category-item.component.html',
  styleUrls: ['./category-item.component.css']
})
export class CategoryItemComponent {
  @Input() category: Category;

  constructor(private categoryService: CategoryService, private router: Router) { }

  onRemove(id: string) {
    this.categoryService.deleteCategory(id).then(() =>
      this.categoryService.getCategories()
    ).catch(error =>
      console.log(error)
    );
  }
}
