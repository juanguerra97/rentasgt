import {Component, OnInit, TemplateRef} from '@angular/core';
import { faPlus, faTrash, faEdit } from '@fortawesome/free-solid-svg-icons';
import { CategoriesClient, CategoryDto } from '../../rentasgt-api';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { PageInfo } from '../../models/PageInfo';
import { ConfirmationModalComponent } from '../../app-common/confirmation-modal/confirmation-modal.component';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {

  faPlus = faPlus;
  faTrash = faTrash;
  faEdit = faEdit;

  public PAGE_SIZE = 5;
  public DEFAULT_PAGE_NUMBER = 1;
  public categories: CategoryDto[] = [];
  public selectedCategory: CategoryDto = null;
  public pageInfo: PageInfo = null;
  public loadingCategories = false;

  private modalNewCategoryRef: BsModalRef|null = null;
  private modalEditCategoryRef: BsModalRef|null = null;

  constructor(
    private categoriesClient: CategoriesClient,
    private bsModalService: BsModalService,
  ) { }

  ngOnInit(): void {
    this.loadCategories(this.PAGE_SIZE, this.DEFAULT_PAGE_NUMBER);
  }

  public loadCategories(pageSize: number, pageNumber: number): void {
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

  public onShowNewCategoryFormModal(template: TemplateRef<any>): void {
    this.modalNewCategoryRef = this.bsModalService.show(template);
  }

  public onNewCategorySaved(category: CategoryDto): void {
    if (category === null || category === undefined) {
      return;
    }
    this.categories.unshift(category);
    this.modalNewCategoryRef.hide();
  }

  public onShowEditCategoryFormModal(template: TemplateRef<any>): void {
    this.modalEditCategoryRef = this.bsModalService.show(template);
  }

  public onCategoryUpdated(category: CategoryDto): void {
    if (category === null || category === undefined) {
      return;
    }
    const catIndex = this.categories.findIndex(cat => cat.id === this.selectedCategory.id);
    if (catIndex >= 0) {
      this.categories[catIndex] = category;
    }
    this.modalEditCategoryRef.hide();
  }

  public onCategoryClicked(category: CategoryDto): void {
    if (this.selectedCategory == null || category.id !== this.selectedCategory.id) {
      this.selectedCategory = category;
    } else {
      this.selectedCategory = null;
    }
  }

  public showDeleteConfirmationModal(): void {
    if (this.selectedCategory === null) {
      return;
    }
    const modal = this.bsModalService.show(ConfirmationModalComponent);
    (<ConfirmationModalComponent>modal.content).showConfirmationModal(
      'Eliminación',
      '¿Estás seguro que quieres eliminar esta categoría?'
    );

    (<ConfirmationModalComponent>modal.content).onClose.subscribe(result => {
      if (result === true) {
        this.categoriesClient.delete(this.selectedCategory.id).subscribe((res) => {
          console.log(res);
          const categIndex = this.categories.findIndex(cat => cat.id === this.selectedCategory.id);
          this.categories.splice(categIndex, 1);
          this.selectedCategory = null;
        }, console.error);
      }
    });
  }

}
