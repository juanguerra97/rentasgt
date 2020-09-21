import { Component, OnInit } from '@angular/core';
import {
  ConflictDto, ConflictRecordDto,
  ConflictRecordsClient,
  ConflictsClient,
  ConflictStatus, CreateConflictRecordCommand,
  UserConflictDto,
  UserProfileDto,
  UsersClient
} from '../../rentasgt-api';
import {PageInfo} from '../../models/PageInfo';
import {DateTime} from 'luxon';
import {FormControl, Validators} from '@angular/forms';
import {SelectItem} from 'primeng';

@Component({
  selector: 'app-conflicts',
  templateUrl: './conflicts.component.html',
  styleUrls: ['./conflicts.component.css']
})
export class ConflictsComponent implements OnInit {

  public DEFAULT_PAGE_NUMBER = 1;
  public PAGE_SIZE = 10;

  public CONFLICT_STATUS_PENDING = ConflictStatus.Pending;
  public CONFLICT_STATUS_CANCELLED = ConflictStatus.Cancelled;
  public CONFLICT_STATUS_IN_PROCESS = ConflictStatus.InProcess;
  public CONFLICT_STATUS_FINISHED = ConflictStatus.Finished;
  public CONFLICT_STATUS_LABELS: string[] = [];

  public pageInfo: PageInfo = null;
  public conflicts: ConflictDto[] = [];
  public loadingConflicts: boolean = false;
  public selectedConflict: ConflictDto = null;
  public conflictRecords: ConflictRecordDto[] = [];
  public loadingConflictRecords: boolean = false;
  public currentUser: UserProfileDto = null;
  public conflictStatus: ConflictStatus = null;

  public statusItems: SelectItem[] = [
    { label: 'Pendientes', value: 0 },
    { label: 'En Proceso', value: 2 },
    { label: 'Terminados', value: 3 }
  ];

  public formControlDescription = new FormControl('', [
    Validators.required,
  ]);

  constructor(
    private conflictsClient: ConflictsClient,
    private conflictRecordsClient: ConflictRecordsClient,
    private usersClient: UsersClient,
  ) { }

  ngOnInit(): void {
    this.loadCurrentUser();
    this.loadConflicts();
    this.CONFLICT_STATUS_LABELS[this.CONFLICT_STATUS_PENDING] = 'Pendiente';
    this.CONFLICT_STATUS_LABELS[this.CONFLICT_STATUS_CANCELLED] = 'Cancelado';
    this.CONFLICT_STATUS_LABELS[this.CONFLICT_STATUS_IN_PROCESS] = 'En proceso';
    this.CONFLICT_STATUS_LABELS[this.CONFLICT_STATUS_FINISHED] = 'Finalizado';
  }

  private loadCurrentUser(): void {
    this.usersClient.getUserProfile()
      .subscribe((res) => {
        this.currentUser = res;
      }, console.error);
  }

  public loadConflicts(pageNumber = this.DEFAULT_PAGE_NUMBER, pageSize = this.PAGE_SIZE): void {
    this.loadingConflicts = true;
    this.conflictsClient.getConflicts(pageSize, pageNumber, undefined, undefined)
      .subscribe((res) => {
        this.pageInfo = new PageInfo(res.currentPage, res.totalPages, res.pageSize, res.totalCount);
        this.conflicts = res.items;
        this.loadingConflicts = false;
      }, error => {
        console.error(error);
        this.loadingConflicts = false;
      });
  }

  public onSelectedConflictChange(): void {
    if (this.selectedConflict) {
      this.loadConflictRecords(this.selectedConflict.id);
    }
  }

  public onDeselectConflict(): void {
    this.conflictRecords = [];
    this.selectedConflict = null;
    this.loadConflicts();
  }

  public loadConflictRecords(idConflict: number): void {
    this.loadingConflictRecords = true;
    this.conflictRecordsClient.getConflictRecords(idConflict)
      .subscribe((res) => {
        this.conflictRecords = res;
        this.loadingConflictRecords = false;
      }, error => {
        console.error(error);
        this.loadingConflictRecords = false;
      });
  }

  public onModerate(): void {
    this.conflictsClient.moderate(this.selectedConflict.id)
      .subscribe((res) => {
        this.selectedConflict.status = ConflictStatus.InProcess;
        this.selectedConflict.moderatorId = this.currentUser.id;
        this.selectedConflict.moderator = new UserConflictDto({
          firstName: this.currentUser.firstName,
          lastName: this.currentUser.lastName,
          email: this.currentUser.email,
          phoneNumber: this.currentUser.phoneNumber,
        });
      }, console.error);
  }

  public onFinish(): void {
    this.conflictsClient.finish(this.selectedConflict.id)
      .subscribe((res) => {
        this.selectedConflict.status = ConflictStatus.Finished;
      }, console.error);
  }

  public onNewConflictRecord(): void {
    this.conflictRecordsClient.create(new CreateConflictRecordCommand({
      conflictId: this.selectedConflict.id,
      description: this.formControlDescription.value
    })).subscribe((res) => {
      this.loadConflictRecords(this.selectedConflict.id);
    }, console.error);
  }

  public getDemandado(): UserConflictDto {
    if (this.selectedConflict.complainantId === this.selectedConflict.rent.request.requestorId) {
      return this.selectedConflict.rent.request.product.owner;
    }
    return this.selectedConflict.rent.request.requestor;
  }

  public formatDate(date: Date): string {
    return DateTime.fromJSDate(date).toFormat('dd/MM/yyyy t');
  }

}
