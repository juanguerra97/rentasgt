import { Component, OnInit } from '@angular/core';
import { CreateRentRequestCommand, ProductDto, ProductsClient, RentRequestsClient, ChatRoomDto, ChatRoomsClient, CreateChatRoomCommand } from '../rentasgt-api';
import { ActivatedRoute, Router } from '@angular/router';
import { DateTime } from 'luxon';
import { AuthorizeService, IUser } from '../../api-authorization/authorize.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {

  public product: ProductDto = null;
  public chatRoom: ChatRoomDto = null;
  public loadingProduct = false;
  public notFound = false;
  public displaySelectDateModal = false;
  public rentDate: Date[] = null;
  public minDate = new Date(DateTime.local().toFormat('MM/dd/yyyy'));
  public maxDate = new Date(DateTime.local().plus({months: 1}).toFormat('MM/dd/yyyy'));
  public esCalendarLocale: any;
  public creatingRequest = false;
  public currentUser: IUser = null;
  public displayMessageModal = false;
  public firstMessage = '';
  public submittingMessage = false;

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
    private rentRequestsClient: RentRequestsClient,
    public authService: AuthorizeService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
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
    this.authService.getUser().subscribe(user => {
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
    }, error => {
        this.loadingProduct = false;
        this.notFound = error.status == 404;
      });
  }

  private loadChatRoom(): void {
    this.chatRoomsClient.getRoomForProduct(this.product.id)
      .subscribe()
  }

  public onMandarMensaje(): void 
  {
    if (this.chatRoom !== null)
    {
      this.redirectToMessages();
    } else {
        this.displayMessageModal = true;
    }
  }

  private redirectToMessages(): void 
  {
    this.router.navigate['/mensajes'];
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
      .subscribe((res) => {
        this.submittingMessage = false;
        this.closeMessageModal();
        this.redirectToMessages();
      }, error => {
        console.error(error);
        this.submittingMessage = false;
      });
  }
  
  public isOwner(): boolean {
    return this.currentUser && this.product.owner.id === this.currentUser.sub;
  }

  public showSelectDateModal(): void {
    this.displaySelectDateModal = true;
  }

  public cancelDateSelection(): void {
    this.rentDate = null;
    this.displaySelectDateModal = false;
  }

  public isValidRentDate(): boolean {
    return !!this.rentDate;
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
        this.displaySelectDateModal = false;
        this.creatingRequest = false;
      }, error => {
        this.creatingRequest = false;
        console.error(error.response);
      });
  }

}
