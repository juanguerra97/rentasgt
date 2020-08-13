import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CategoriesClient, CategoryDto } from '../../../rentasgt-api';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { getErrorsFromResponse } from '../../../utils';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})
export class EditCategoryComponent implements OnInit {

  @Input() category: CategoryDto = null;
  @Output() onCategoryUpdated: EventEmitter<CategoryDto> = new EventEmitter<CategoryDto>();
  @Output() onError: EventEmitter<any> = new EventEmitter<any>();

  public saving = false;
  public errorMsg: string = null;

  public editCategoryForm = new FormGroup({
    name: new FormControl('', [
      Validators.required,
      Validators.maxLength(128),
    ]),
    description: new FormControl('', [
      Validators.required,
      Validators.maxLength(512),
    ])
  });

  constructor(
    private categoriesClient: CategoriesClient,
  ) { }

  ngOnInit(): void {
    if (this.category != null) {
      this.editCategoryForm.reset(this.category);
    }
  }

  public onSubmitEditCategoryForm(): void {
    if (!this.editCategoryForm.valid) {
      return;
    }
    this.errorMsg = null;
    this.saving = true;
    const cat = Object.assign({ id: this.category.id }, this.editCategoryForm.value);
    this.categoriesClient.update(this.category.id, cat).subscribe((res) => {
      const updatedCategory: CategoryDto = Object.assign({ id: this.category.id }, this.editCategoryForm.value);
      this.editCategoryForm.reset();
      this.saving = false;
      this.onCategoryUpdated.emit(updatedCategory);
    }, (error) => {
      this.saving = false;
      const errors = getErrorsFromResponse(JSON.parse(error.response));
      this.onError.emit(errors);
      if (errors.length > 0) {
        this.errorMsg = errors[0];
      }
    });
  }

}
