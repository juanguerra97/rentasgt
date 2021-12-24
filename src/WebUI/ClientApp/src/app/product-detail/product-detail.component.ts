import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CreateRentRequestCommand, ProductDto, ProductsClient, RentRequestsClient, ChatRoomDto, ChatRoomsClient, CreateChatRoomCommand, UserProfileStatus, UserProfileDto, UsersClient } from '../rentasgt-api';
import { DateTime, Duration } from 'luxon';
import { AuthorizeService, IUser } from '../../api-authorization/authorize.service';
import { getErrorsFromResponse } from '../utils';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {

  public PROFILE_INCOMPLETE = UserProfileStatus.Incomplete;
  public PROFILE_WAITING_FOR_APPROVAL = UserProfileStatus.WaitingForApproval;
  public PROFILE_ACTIVE = UserProfileStatus.Active;

  public product: ProductDto = null;
  public chatRoom: ChatRoomDto = null;
  public loadingProduct = true;
  public notFound = false;
  public displaySelectDateModal = false;
  public rentDate: Date[] = null;
  public minDate = new Date(DateTime.local().toFormat('MM/dd/yyyy'));
  public maxDate = new Date(DateTime.local().plus({months: 1}).toFormat('MM/dd/yyyy'));
  public esCalendarLocale: any;
  public creatingRequest = false;
  public currentUser: UserProfileDto = null;
  public displayMessageModal = false;
  public firstMessage = '';
  public submittingMessage = false;
  public messageModalTitle: string = '';
  public estimatedCost = 0;
  public rentError: string = null;
  public reservedDates: Date[] = [];

  responsiveOptions: any[] = [
    {
      breakpoint: '1024px',
      numVisible: 5
    },
    {
      breakpoint: '768px',
      numVisible: 3
    },
    {
      breakpoint: '560px',
      numVisible: 1
    }
  ];

  constructor(
    private productsClient: ProductsClient,
    private chatRoomsClient: ChatRoomsClient,
    private usersClient: UsersClient,
    private rentRequestsClient: RentRequestsClient,
    public authService: AuthorizeService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private messageService: MessageService,
  ) { }

  ngOnInit() {
    this.activatedRoute.paramMap.subscribe(async (params) => {
      const id = Number.parseInt(params.get('id'));
      if (!id) {
        await this.router.navigate(['/articulos']);
      } else {
        this.loadProduct(id);
      }
    });
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
    this.usersClient.getUserProfile().subscribe(user => {
      this.currentUser = user;
    }, console.error);
  }

  private loadProduct(productId: number): void {
    this.loadingProduct = true;
    this.productsClient.getById(productId)
      .subscribe((res) => {
        this.product = res;
        this.loadingProduct = false;
        this.loadChatRoom();
        this.loadReservedDates(productId);
    }, error => {
        this.loadingProduct = false;
        this.notFound = error.status == 404;
      });
  }

  private loadChatRoom(): void {
    this.chatRoomsClient.getRoomForProduct(this.product.id)
      .subscribe(async (res) => {
        this.chatRoom = res;
      }, console.error);
  }

  private loadReservedDates(productId: number): void {
    this.productsClient.getReservedDatesForNextMonth(productId)
      .subscribe((res) => {
        this.reservedDates = res;
      }, console.error);
  }

  public async onMandarMensaje(): Promise<any> {
    if (this.chatRoom !== null) {
      await this.router.navigate(['/mensajes', {
        roomId: this.chatRoom.id
      }]);
    } else {
        this.displayMessageModal = true;
        this.messageModalTitle = `Envíale un mensaje a ${this.product.owner.firstName}`;
    }
  }

  public closeMessageModal(): void {
    this.displayMessageModal = false;
  }

  public onSubmitMessage(): void {
    this.submittingMessage = true;
    const firstMessageCommand = new CreateChatRoomCommand();
    firstMessageCommand.firstMessage = this.firstMessage;
    firstMessageCommand.productId = this.product.id;
    this.chatRoomsClient.create(firstMessageCommand)
      .subscribe(async (res) => {
        this.submittingMessage = false;
        this.closeMessageModal();
        await this.router.navigate(['/mensajes', {
          roomId: res,
          // msg: `Hola ${this.product.owner.firstName}! He visto tu anuncio de "${this.product.name}" y estoy interesado.`
        }]);
      }, error => {
        console.error(error);
        this.submittingMessage = false;
      });
  }

  public isOwner(): boolean {
    return this.currentUser && this.product.owner.id === this.currentUser.id;
  }

  public showSelectDateModal(): void {
    this.messageService.clear('msgsCreateRentRequest');
    this.displaySelectDateModal = true;
  }

  public cancelDateSelection(): void {
    this.rentDate = null;
    this.displaySelectDateModal = false;
  }

  public isValidRentDate(): boolean {
    return this.rentDate && this.rentDate.length === 2 && !!this.rentDate[0] && !!this.rentDate[1];
  }

  public onCreateRentRequest(): void {
    if (this.rentDate === null || this.rentDate.length === 0 ) {
      return;
    }
    this.creatingRequest = true;
    const start = this.rentDate[0];
    let end = start;
    if (this.rentDate.length > 1) {
      end = this.rentDate[1];
    }
    const req = new CreateRentRequestCommand();
    req.productId = this.product.id;
    req.startDate = start;
    req.endDate = end;
    this.rentRequestsClient.create(req)
      .subscribe((res) => {
        this.rentDate = [];
        this.loadReservedDates(this.product.id);
        this.displaySelectDateModal = false;
        this.creatingRequest = false;
      }, error => {
        this.creatingRequest = false;
        if (error.response) {
          const err = getErrorsFromResponse(JSON.parse(error.response))[0];
          this.messageService.add({key: 'msgsCreateRentRequest', severity: 'error', detail: err});
        }
        
      });
  }

  public onDateSelected($event): void {
    if (this.rentDate && this.rentDate.length === 2 && this.rentDate[0] && this.rentDate[1]) {
      const d = DateTime.fromJSDate(this.rentDate[0]).diff(DateTime.fromJSDate(this.rentDate[1]), 'days');
      const days = Math.abs(d.days) + 1;
      this.estimatedCost = this.getEstimatedCost(days);
    } else if (this.rentDate && this.rentDate.length === 1 && this.rentDate[0]) {
      this.estimatedCost = this.product.costPerDay;
    } else {
      this.estimatedCost = 0;
    }
  }

  public getEstimatedCost(days: number): number {
    let cost = 0;
    let d = days;
    if (this.product.costPerMonth) {
      const months = Math.floor(days / 31);
      d = d - (months * 31);
      cost += this.product.costPerMonth * months;
    }
    if (this.product.costPerWeek) {
      const weeks = Math.floor(d / 7);
      d = d - (weeks * 7);
      cost += this.product.costPerWeek * weeks;
    }
    cost += d * this.product.costPerDay;
    return cost;
  }

}
