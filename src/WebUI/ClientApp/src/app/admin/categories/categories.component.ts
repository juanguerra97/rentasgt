import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { faTrash, faEdit } from '@fortawesome/free-solid-svg-icons';
import { CategoriesClient, CategoryDto } from '../../rentasgt-api';
import { PageInfo } from '../../models/PageInfo';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {

  faTrash = faTrash;
  faEdit = faEdit;

  public PAGE_SIZE = 5;
  public DEFAULT_PAGE_NUMBER = 1;
  public categories: CategoryDto[] = [];
  public selectedCategory: CategoryDto = null;
  public pageInfo: PageInfo = null;
  public loadingCategories = false;

  public newCategoryForm = new FormGroup({
    name: new FormControl('', [
      Validators.required
    ]),
    description: new FormControl('', [
      Validators.required
    ])
  });

  constructor(
    private categoriesClient: CategoriesClient
  ) { }

  ngOnInit(): void {
    this.loadCategories(this.PAGE_SIZE, this.DEFAULT_PAGE_NUMBER);
  }

  private loadCategories(pageSize: number, pageNumber: number): void {
    this.loadingCategories = true;
    this.categoriesClient.get(pageSize, pageNumber).subscribe((res => {
      this.pageInfo = new PageInfo(res.currentPage, res.totalPages, res.pageSize, res.totalCount);
      this.categories = res.items;
      this.loadingCategories = false;
    }), error => {
      this.loadingCategories = false;
      console.error(error);
    });
  }

  public onSubmitNewCategoryForm(): void {
    if (!this.newCategoryForm.valid) {
      return;
    }
    this.categoriesClient.create(this.newCategoryForm.value).subscribe((res) => {
      const newCategory: CategoryDto = Object.assign({}, this.newCategoryForm.value, {id: res});
      this.categories.unshift(newCategory);
      this.newCategoryForm.reset();
    }, console.error);
  }

  public onCategoryClicked(category: CategoryDto): void {
    if (this.selectedCategory == null || category.id !== this.selectedCategory.id) {
      this.selectedCategory = category;
    } else {
      this.selectedCategory = null;
    }
  }

}
