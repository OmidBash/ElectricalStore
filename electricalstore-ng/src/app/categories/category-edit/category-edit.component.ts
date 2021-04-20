import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Category } from '../category.model';
import { CategoryService } from '../category.service';

@Component({
  selector: 'app-category-edit',
  templateUrl: './category-edit.component.html',
  styleUrls: ['./category-edit.component.css'],
  host: {
    class: 'editor'
  }
})
export class CategoryEditComponent implements OnInit {
  category = new Category();
  editMode = false;
  @ViewChild('editForm', { static: false }) editForm: NgForm;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private categoryService: CategoryService) { }

  ngOnInit(): void {
    this.route.data.subscribe((category: Category) => {
      this.category = category[0];
      this.editMode = this.category.id !== undefined;
    });
  }
  onSubmitForm() {
    if (this.editMode) {
      this.categoryService.updateCategory(this.editForm.form.value).then(
        () => {
          this.categoryService.getCategories();
          this.reset();
        },
        error => {
          console.log(error);
        });
    } else {
      this.categoryService.addCategory(this.editForm.form.value).then(
        () => {
          this.categoryService.getCategories();
          this.reset();
        },
        error => {
          console.log(error);
        }
      );
    }
  }

  reset() {
    this.editForm.reset();
    this.editMode = false;
    this.router.navigate(['../'], { relativeTo: this.route });
  }

  onCancelForm() {
    this.reset();
  }
}
