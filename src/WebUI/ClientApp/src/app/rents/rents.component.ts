import { Component, OnInit } from '@angular/core';
import { RentDto, RentRequestDto, RentRequestRentDto, RentsClient, RentStatus}  from '../rentasgt-api';
import {PageInfo} from '../models/PageInfo';
import { AuthorizeService, IUser } from 'src/api-authorization/authorize.service';
import { BsModalService } from 'ngx-bootstrap';
import { ConfirmationModalComponent } from '../app-common/confirmation-modal/confirmation-modal.component';
import {DateTime} from 'luxon';

@Component({
  selector: 'app-rents',
  templateUrl: './rents.component.html',
  styleUrls: ['./rents.component.css']
})
export class RentsComponent implements OnInit {

  PAGE_SIZE = 10;
  DEFAULT_PAGE_NUMBER = 1;

  public RENT_STATUS_LABELS: string[] = [];
  public RENT_STATUS_PENDING = RentStatus.Pending;
  public RENT_STATUS_DELIVERED = RentStatus.ProductDelivered;
  public RENT_STATUS_RETURNED = RentStatus.ProductReturned;
  public RENT_STATUS_RETURN_DELAYED = RentStatus.ReturnDelayed;
  public RENT_STATUS_CANCELLED = RentStatus.Cancelled;
  public RENT_STATUS_CONFLICT = RentStatus.Conflict;
  public RENT_STATUS_NOT_COMPLETED = RentStatus.NotCompleted;

  public pageInfo: PageInfo = null;
  public rentRequests: RentRequestRentDto[] = [];
  public selectedRentRequest: RentRequestRentDto = null;
  public loadingRents: boolean = false;
  public currentUser: IUser = null;

  constructor(
    private rentsClient: RentsClient,
    private authService: AuthorizeService,
    private bsModalService: BsModalService,
  ) { }

  ngOnInit(): void {
    this.loadCurrentUser();
    this.loadRents();
    this.RENT_STATUS_LABELS[RentStatus.Pending] = 'Pendiente';
    this.RENT_STATUS_LABELS[RentStatus.ProductDelivered] = 'En proceso';
    this.RENT_STATUS_LABELS[RentStatus.ProductReturned] = 'Finalizada';
    this.RENT_STATUS_LABELS[RentStatus.Cancelled] = 'Cancelada';
    this.RENT_STATUS_LABELS[RentStatus.ReturnDelayed] = 'Devolución demorada';
    this.RENT_STATUS_LABELS[RentStatus.NotCompleted] = 'Sin completar';
    this.RENT_STATUS_LABELS[RentStatus.Conflict] = 'En conflicto';
  }

  public loadCurrentUser(): void {
    this.authService.getUser().subscribe((user) => {
      this.currentUser = user;
    }, console.error);
  }

  public loadRents(pageSize = this.PAGE_SIZE, pageNumber = this.DEFAULT_PAGE_NUMBER): void {
    this.loadingRents = true;
    this.rentsClient.getOfRequestor(pageSize, pageNumber)
      .subscribe((res) => {
        this.rentRequests = res.items;
        this.pageInfo = new PageInfo(res.currentPage, res.totalPages, res.pageSize, res.totalCount);
        this.loadingRents = false;
      }, error => {
        console.error(error);
        this.loadingRents = false;
      });
  }

  public selectRentRequest(rentRequest: RentRequestRentDto): void {
    this.selectedRentRequest = rentRequest;
    if (!rentRequest) {
      this.loadRents();
    }
  }

  public confirmCancelRent(): void {
    const modal = this.bsModalService.show(ConfirmationModalComponent);
    (<ConfirmationModalComponent>modal.content).showConfirmationModal(
      'Cancelar renta',
      `¿Estás seguro que quieres cancelar la renta de este artículo?`
    );

    (<ConfirmationModalComponent>modal.content).onClose.subscribe(result => {
      if (result === true) {
        this.cancelRent();
      }
    });
  }

  public cancelRent(): void {
    this.rentsClient.cancelRent(this.selectedRentRequest.id)
      .subscribe((res) => {
        this.selectedRentRequest.rent.status = this.RENT_STATUS_CANCELLED;
      }, console.error);
  }

  public confirmStartRent(): void {
    const modal = this.bsModalService.show(ConfirmationModalComponent);
    (<ConfirmationModalComponent>modal.content).showConfirmationModal(
      'Empezar renta',
      `¿Estás seguro que ya has recibido este artículo?`
    );

    (<ConfirmationModalComponent>modal.content).onClose.subscribe(result => {
      if (result === true) {
        this.startRent();
      }
    });
  }

  public startRent(): void {
    this.rentsClient.startRent(this.selectedRentRequest.id)
      .subscribe((res) => {
        this.selectedRentRequest.rent.status = this.RENT_STATUS_DELIVERED
      }, console.error);
  }

  public formatDateShort(date: Date): string {
    return DateTime.fromJSDate(date).toFormat('dd/MM/yyyy');
  }

}