import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import {EndRentCommand, RentRequestRentDto, RentsClient, RentStatus} from '../../rentasgt-api';

@Component({
  selector: 'app-end-rent',
  templateUrl: './end-rent.component.html',
  styleUrls: ['./end-rent.component.css']
})
export class EndRentComponent implements OnInit {

  public ratingValue: number = null;

  constructor(
    private rentsClient: RentsClient,
    private dialogRef: MatDialogRef<EndRentComponent>,
    @Inject(MAT_DIALOG_DATA) public data: RentRequestRentDto,
  ) { }

  ngOnInit(): void {

  }

  public onEndRent(): void {
    this.rentsClient.endRent(this.data.id, new EndRentCommand({ rentId: this.data.id, ratingValue: this.ratingValue }))
      .subscribe((res) => {
        this.data.rent.status = RentStatus.ProductReturned;
        this.dialogRef.close(true);
      }, console.error);
  }

}
