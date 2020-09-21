import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {ConflictModalData} from '../../models/ConflictModalData';
import {ConflictsClient, CreateConflictCommand} from '../../rentasgt-api';

@Component({
  selector: 'app-create-conflict',
  templateUrl: './create-conflict.component.html',
  styleUrls: ['./create-conflict.component.css']
})
export class CreateConflictComponent implements OnInit {

  public description: string = '';

  constructor(
    public conflictsClient: ConflictsClient,
    public dialogRef: MatDialogRef<CreateConflictComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ConflictModalData
  ) { }

  ngOnInit(): void {
  }

  public onReportRent(): void {
    const command = new CreateConflictCommand({ description: this.description, rentId: this.data.rent.requestId });
   this.conflictsClient.create(command)
     .subscribe((res) => {
       this.dialogRef.close(res);
     }, console.error);
  }

  public isValidDescription(): boolean {
    return this.description && this.description.trim().length > 0;
  }

}
