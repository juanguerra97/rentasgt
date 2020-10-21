import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { API_BASE_URL, RateProductCommand, RatingToProductDto, RatingToProductsClient } from '../../rentasgt-api';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-rate-product',
  templateUrl: './rate-product.component.html',
  styleUrls: ['./rate-product.component.css']
})
export class RateProductComponent implements OnInit {

  public savingRating: boolean = false;
  public ratingToProductForm = new FormGroup({
    ratingToProductValue: new FormControl('', [
      Validators.required,
    ]),
    ratingToOwnerValue: new FormControl('', [
      Validators.required,
    ]),
    comment: new FormControl('', []),
  });

  constructor(
    private ratingToProductsClient: RatingToProductsClient,
    private dialogRef: MatDialogRef<RateProductComponent>,
    @Inject(MAT_DIALOG_DATA) public data: RatingToProductDto,
    @Inject(API_BASE_URL) public baseUrl?: string,
  ) { }

  ngOnInit(): void {

  }

  public onSubmitRatingToProduct(): void {
    this.savingRating = true;
    const value = this.ratingToProductForm.value;
    this.ratingToProductsClient.rate(this.data.id, new RateProductCommand({
      ratingId: this.data.id,
      ownerRatingValue: value.ratingToOwnerValue,
      productRatingValue: value.ratingToProductValue,
      comment: (value.comment.trim().length === 0 ? undefined : value.comment)
    })).subscribe((res) => {
      this.ratingToProductForm.reset();
      this.savingRating = false;
      this.dialogRef.close();
    }, error => {
      console.error(error);
      this.savingRating = false;
    });
  }

  public onIgnoreRating(): void {
    this.savingRating = true;
    this.ratingToProductsClient.ignore(this.data.id)
      .subscribe((res) => {
        this.dialogRef.close();
        this.savingRating = false;
      }, error => {
        console.log(error);
        this.savingRating = false;
      });
  }

}
