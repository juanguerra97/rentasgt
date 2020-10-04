import { Component, OnInit, TemplateRef } from '@angular/core';
import {
  RejectRentRequestCommand,
  RentRequestDto,
  RentRequestsClient,
  RequestEventDto,
  RequestEventType,
  RequestStatus
} from '../../rentasgt-api';
import { PageInfo } from '../../models/PageInfo';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ConfirmationModalComponent } from '../../app-common/confirmation-modal/confirmation-modal.component';
import { DateTime } from 'luxon';
import {MessageService} from 'primeng';
import {getErrorsFromResponse} from '../../utils';

@Component({
  selector: 'app-rent-requests',
  templateUrl: './rent-requests.component.html',
  styleUrls: ['./rent-requests.component.css']
})
export class RentRequestsComponent implements OnInit {

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

  public esCalendarLocale: any;

  public pageInfo: PageInfo = null;
  public rentRequests: RentRequestDto[] = [];
  public loadingRentRequests: boolean = true;
  public selectedRentRequest: RentRequestDto = null;
  public rentRequestEvents: RequestEventDto[] = [];
  public rentDate: Date[] = null;
  public loadingRequestEvents: boolean = false;
  public message: string = '';

  public modalReject: BsModalRef;

  constructor(
    private rentRequestsClient: RentRequestsClient,
    private bsModalService: BsModalService,
    private messageService: MessageService,
  ) { }

  ngOnInit(): void {
    this.loadRentRequests();

    this.REQ_STATUS_LABELS[RequestStatus.Pending] = 'Pendiente';
    this.REQ_STATUS_LABELS[RequestStatus.Viewed] = 'Vista';
    this.REQ_STATUS_LABELS[RequestStatus.Cancelled] = 'Cancelada';
    this.REQ_STATUS_LABELS[RequestStatus.NotResolved] = 'Sin resolver';
    this.REQ_STATUS_LABELS[RequestStatus.Rejected] = 'Rechazada';
    this.REQ_STATUS_LABELS[RequestStatus.Accepted] = 'Aceptada';

    this.EVENT_TYPE_CLASSES[this.REQUEST_EVENT_VIEWED] = 'rq-viewed';
    this.EVENT_TYPE_CLASSES[this.REQUEST_EVENT_CANCELLED] = 'rq-cancelled';
    this.EVENT_TYPE_CLASSES[this.REQUEST_EVENT_ACCEPTED] = 'rq-accepted';
    this.EVENT_TYPE_CLASSES[this.REQUEST_EVENT_REJECTED] = 'rq-rejected';

    this.esCalendarLocale = {
      firstDayOfWeek: 0,
      dayNames: ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"],
      dayNamesShort: ["Dom", "Lun", "Mar", "Mie", "Jue", "Vie", "Sab"],
      dayNamesMin: ["Do","Lu","Ma","Mi","Ju","Vi","Sa"],
      monthNames: [ "Enero","Febrero","Marzo","Abril","Mayo","Junio","Julio","Agosto","Septiembre","Octubre","Noviembre","Diciembre" ],
      monthNamesShort: [ "Ene", "Feb", "Mar", "Abr", "May", "Jun","Jul", "Ago", "Sep", "Oct", "Nov", "Dic" ],
      today: 'Hoy',
      clear: 'Borrar',
      dateFormat: 'mm/dd/yy',
      weekHeader: 'Sem'
    };
  }

  public loadRentRequests(pageSize = this.DEFAULT_PAGE_SIZE, pageNumber = this.DEFAULT_PAGE_NUMBER): void {
    this.loadingRentRequests = true;
    this.rentRequestsClient.getOfOwner(pageSize, pageNumber)
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
      if (rq.status === this.REQUEST_STATUS_PENDING) {
        this.rentRequestsClient.view(rq.id)
          .subscribe((res) => {
            this.selectedRentRequest.status = this.REQUEST_STATUS_VIEWED;
            this.loadRentRequestHistory(rq.id);
          }, console.error);
      } else {
        this.loadRentRequestHistory(rq.id);
      }
      this.rentDate = [this.selectedRentRequest.startDate, this.selectedRentRequest.endDate];
    } else {
      this.rentDate = null;
      this.loadRentRequests(this.pageInfo.pageSize, this.pageInfo.currentPage);
    }
  }

/*  public confirmCancelRequest(): void {
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
  }*/


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

  public openRejectModal(modal: TemplateRef<any>): void {
   this.modalReject = this.bsModalService.show(modal);
  }

  public hideModalReject(): void {
    this.modalReject.hide();
  }

  public confirmAcceptRentRequest(): void {
    const modal = this.bsModalService.show(ConfirmationModalComponent);
    (<ConfirmationModalComponent>modal.content).showConfirmationModal(
      'Aceptar solicitud',
      `¿Estás seguro que quieres aceptar la solicitud de ${this.selectedRentRequest.requestor.firstName}? Si aceptas te estás comprometiendo a rentarle tu artículo.`
    );

    (<ConfirmationModalComponent>modal.content).onClose.subscribe(result => {
      if (result === true) {
        this.acceptRequest();
      }
    });
  }

  public acceptRequest(): void {
    this.rentRequestsClient.accept(this.selectedRentRequest.id)
      .subscribe((res) => {
        this.selectedRentRequest.status = this.REQUEST_STATUS_ACCEPTED;
        this.loadRentRequestHistory(this.selectedRentRequest.id);
      }, error => {
        console.error(error);
        if (error.response) {
          const errors = getErrorsFromResponse(JSON.parse(error.response));
          if (errors.length > 0) {
            this.messageService.add({severity: 'error', detail: errors[0]});
          }
        }
      });
  }

  public rejectRequest(): void {
    const rejectCommand = new RejectRentRequestCommand();
    rejectCommand.rentRequestId = this.selectedRentRequest.id;
    rejectCommand.message = this.message;
    this.rentRequestsClient.reject(rejectCommand.rentRequestId, rejectCommand)
      .subscribe((res) => {
        this.selectedRentRequest.status = this.REQUEST_STATUS_REJECTED;
        this.loadRentRequestHistory(this.selectedRentRequest.id);
        this.modalReject.hide();
        this.message = '';
      }, error => {
        console.error(error);
        if (error.response) {
          const errors = getErrorsFromResponse(JSON.parse(error.response));
          if (errors.length > 0) {
            this.messageService.add({severity: 'error', detail: errors[0]});
          }
        }
      });
  }

  public requestCanBeAccepted(): boolean {
    return this.selectedRentRequest.status === this.REQUEST_STATUS_PENDING
      || this.selectedRentRequest.status === this.REQUEST_STATUS_VIEWED;
  }

  public requestCanBeRejected(): boolean {
    return this.selectedRentRequest.status === this.REQUEST_STATUS_PENDING
      || this.selectedRentRequest.status === this.REQUEST_STATUS_VIEWED;
  }

  public formatDate(date: Date): string {
    return DateTime.fromJSDate(date).toFormat('dd/MM/yyyy t');
  }

}
