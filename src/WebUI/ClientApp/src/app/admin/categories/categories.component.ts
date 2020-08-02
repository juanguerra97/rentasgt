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

  static PAGINATOR_SIZE = 5;
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

  public previousPage(): void {
    const firstAvailablePage = this.pageInfo.pagesSpace[0];
    if (firstAvailablePage === 1) {
      return;
    }
    let previous = firstAvailablePage - (Math.floor(this.pageInfo.PAGINATOR_SIZE / 2));
    if (previous < 1) {
      previous = 1;
    }
    this.loadCategories(CategoriesComponent.DEFAULT_PAGE_SIZE, previous);
  }

  public nextPage(): void {
    const lastAvailablePage = this.pageInfo.pagesSpace[this.pageInfo.pagesSpace.length - 1];
    if (lastAvailablePage === this.pageInfo.totalPages) {
      return;
    }
    let next = lastAvailablePage + Math.floor(this.pageInfo.PAGINATOR_SIZE / 2);
    console.log(lastAvailablePage);
    if (next > this.pageInfo.totalPages) {
      next = this.pageInfo.totalPages;
    }
    this.loadCategories(CategoriesComponent.DEFAULT_PAGE_SIZE, next);
  }


}


class PageInfo {

  public PAGINATOR_SIZE = 5;
  public pagesSpace: number[] = [];

  constructor(
    public currentPage: number,
    public totalPages: number,
    public pageSize: number,
    public totalCount: number
  ) {
    this.createPagesSpace();
  }

  private createPagesSpace(): void {
    this.pagesSpace = [];
    let min = this.currentPage - (Math.floor(this.PAGINATOR_SIZE / 2));
    let max = this.currentPage + (Math.floor(this.PAGINATOR_SIZE / 2));
    if (min < 1) {
      if (max < this.totalPages) {
        max += (Math.abs(min - 1));
      }
        min = 1;
    }

    if (max > this.totalPages) {
      if (min > 1) {
        min -= (Math.abs(max - this.totalPages));
        if (min < 1) {
          min = 1;
        }
      }
      max = this.totalPages;
    }

    this.pagesSpace = [];
    for (let page = min; page <= max; ++page) {
      this.pagesSpace.push(page);
    }

  }

}
