import {Component, OnInit, Output, EventEmitter} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CategoriesClient, CategoryDto } from '../../../rentasgt-api';

@Component({
  selector: 'app-new-category',
  templateUrl: './new-category.component.html',
  styleUrls: ['./new-category.component.css']
})
export class NewCategoryComponent implements OnInit {

  @Output() onNewCategorySaved: EventEmitter<CategoryDto> = new EventEmitter<CategoryDto>();
  @Output() onError: EventEmitter<any> = new EventEmitter<any>();

  public saving = false;

  public newCategoryForm = new FormGroup({
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
  }

  public onSubmitNewCategoryForm(): void {
    if (!this.newCategoryForm.valid) {
      return;
    }
    this.saving = true;
    this.categoriesClient.create(this.newCategoryForm.value).subscribe((res) => {
      const newCategory: CategoryDto = Object.assign({}, this.newCategoryForm.value, { id: res });
      this.newCategoryForm.reset();
      this.saving = false;
      this.onNewCategorySaved.emit(newCategory);
    }, error => {
      this.saving = false;
      this.onError.emit(error);
      console.error(error);
    });
  }

}
