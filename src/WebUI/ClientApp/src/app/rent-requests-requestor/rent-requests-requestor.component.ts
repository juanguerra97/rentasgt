import { Component, OnInit } from '@angular/core';
import { RentRequestDto, RentRequestsClient, RequestEventDto, RequestEventType, RequestStatus } from '../rentasgt-api';
import { PageInfo } from '../models/PageInfo';
import { DateTime } from 'luxon';
import { BsModalService } from 'ngx-bootstrap';
import { ConfirmationModalComponent } from '../app-common/confirmation-modal/confirmation-modal.component';

@Component({
  selector: 'app-rent-requests-requestor',
  templateUrl: './rent-requests-requestor.component.html',
  styleUrls: ['./rent-requests-requestor.component.css']
})
export class RentRequestsRequestorComponent implements OnInit {

  DEFAULT_PAGE_NUMBER = 1;
  DEFAULT_PAGE_SIZE = 10;
  REQ_STATUS_LABELS: string[] = [];
  EVENT_TYPE_CLASSES: string[] = [];

  REQUEST_STATUS_PENDING = RequestStatus.Pending;
  REQUEST_STATUS_VIEWED = RequestStatus.Viewed;
  REQUEST_STATUS_CANCELLED = RequestStatus.Cancelled;
  REQUEST_STATUS_NOTRESOLVED = RequestStatus.NotResolved;
  REQUEST_STATUS_REJECTED = RequestStatus.Rejected;
  REQUEST_STATUS_ACCEPTED = RequestStatus.Accepted;

  REQUEST_EVENT_VIEWED = RequestEventType.RequestViewed;
  REQUEST_EVENT_CANCELLED = RequestEventType.RequestCancelled;
  REQUEST_EVENT_ACCEPTED = RequestEventType.RequestAccepted;
  REQUEST_EVENT_REJECTED = RequestEventType.RequestRejected;

  public pageInfo: PageInfo = null;
  public rentRequests: RentRequestDto[] = [];
  public loadingRentRequests: boolean = true;
  public selectedRentRequest: RentRequestDto = null;
  public rentRequestEvents: RequestEventDto[] = [];
  public loadingRequestEvents: boolean = false;
  public cancellingRequest: boolean = false;

  constructor(
    private rentRequestsClient: RentRequestsClient,
    private bsModalService: BsModalService,
  ) { }

  ngOnInit(): void {
    this.loadRentRequests();

    this.REQ_STATUS_LABELS[RequestStatus.Pending] = 'Pendiente';
    this.REQ_STATUS_LABELS[RequestStatus.Viewed] = 'Evaluación';
    this.REQ_STATUS_LABELS[RequestStatus.Cancelled] = 'Cancelada';
    this.REQ_STATUS_LABELS[RequestStatus.NotResolved] = 'Sin resolver';
    this.REQ_STATUS_LABELS[RequestStatus.Rejected] = 'Rechazada';
    this.REQ_STATUS_LABELS[RequestStatus.Accepted] = 'Aceptada';

    this.EVENT_TYPE_CLASSES[this.REQUEST_EVENT_VIEWED] = 'rq-viewed';
    this.EVENT_TYPE_CLASSES[this.REQUEST_EVENT_CANCELLED] = 'rq-cancelled';
    this.EVENT_TYPE_CLASSES[this.REQUEST_EVENT_ACCEPTED] = 'rq-accepted';
    this.EVENT_TYPE_CLASSES[this.REQUEST_EVENT_REJECTED] = 'rq-rejected';
  }

  public loadRentRequests(pageSize = this.DEFAULT_PAGE_SIZE, pageNumber = this.DEFAULT_PAGE_NUMBER): void {
    this.loadingRentRequests = true;
    this.rentRequestsClient.getOfRequestor(pageSize, pageNumber)
      .subscribe((res) => {
        this.rentRequests = res.items;
        this.pageInfo = new PageInfo(res.currentPage, res.totalPages, res.pageSize, res.totalCount);
        this.loadingRentRequests = false;
      }, error => {
        console.error(error);
        this.loadingRentRequests = false;
      });
  }

  public selectRentRequest(rq: RentRequestDto): void {
    this.selectedRentRequest = rq;
    if (rq) {
      this.loadRentRequestHistory(rq.id);
    } else {
      this.loadRentRequests(this.pageInfo.pageSize, this.pageInfo.currentPage);
    }
  }

  public confirmCancelRequest(): void {
    const modal = this.bsModalService.show(ConfirmationModalComponent);
    (<ConfirmationModalComponent>modal.content).showConfirmationModal(
      'Cancelación',
      '¿Estás seguro que quieres cancelar tu solicitud?'
    );

    (<ConfirmationModalComponent>modal.content).onClose.subscribe(result => {
      if (result === true) {
        this.cancelRequest();
      }
    });
  }

  public cancelRequest(): void {
    this.cancellingRequest = true;
    this.rentRequestsClient.cancel(this.selectedRentRequest.id)
      .subscribe((res) => {
        console.log(res);
        this.selectedRentRequest.status = this.REQUEST_STATUS_CANCELLED;
        this.loadRentRequestHistory(this.selectedRentRequest.id);
        this.cancellingRequest = false;
      }, error => {
        console.error(error);
        this.cancellingRequest = false;
      });
  }

  public loadRentRequestHistory(rentRequestId: number): void {
    this.loadingRequestEvents = true;
    this.rentRequestsClient.getHistory(rentRequestId)
      .subscribe((res) => {
        this.rentRequestEvents = res;
        this.loadingRequestEvents = false;
      }, error => {
        console.error(error);
        this.loadingRequestEvents = false;
      });
  }

  public requestCanBeCancelled(): boolean {
    return this.selectedRentRequest.status === this.REQUEST_STATUS_PENDING
      || this.selectedRentRequest.status === this.REQUEST_STATUS_VIEWED;
  }

  public formatDate(date: Date): string {
    return DateTime.fromJSDate(date).toFormat('dd/MM/yyyy t');
  }

}
