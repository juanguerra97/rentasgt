import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { faTrash, faEdit } from '@fortawesome/free-solid-svg-icons';
import { CategoriesClient, CategoryDto } from '../../rentasgt-api';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {

  static DEFAULT_PAGE_SIZE = 5;
  static DEFAULT_PAGE_NUMBER = 1;

  faTrash = faTrash;
  faEdit = faEdit;

  public categories: CategoryDto[] = [];
  public selectedCategory: CategoryDto = null;
  public pageInfo: PageInfo = null;

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
    this.loadCategories(CategoriesComponent.DEFAULT_PAGE_SIZE, CategoriesComponent.DEFAULT_PAGE_NUMBER);
  }

  private loadCategories(pageSize: number, pageNumber: number): void {
    this.categoriesClient.get(pageSize, pageNumber).subscribe((res => {
      this.pageInfo = new PageInfo(res.currentPage, res.totalPages, res.pageSize, res.totalCount);
      this.categories = res.items;
    }), console.error);
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


class PageInfo {
  public pagesSpace: number[];

  constructor(
    public currentPage: number,
    public totalPages: number,
    public pageSize: number,
    public totalCount: number
  ) {
    this.calculatePageSpace();
  }

  private calculatePageSpace(): void {
    let min = 1;
    let max = this.totalPages;
    if (this.totalPages > 5) {
      if (this.currentPage - 2 > 0) {
        min = this.currentPage - 2;
      }
      if (this.currentPage + 2 <= this.totalPages) {
        max = this.currentPage + 2;
      }
    }
    this.pagesSpace = [];
    for (let i = min; i <= max; ++i) {
      this.pagesSpace.push(i);
    }
  }

}
