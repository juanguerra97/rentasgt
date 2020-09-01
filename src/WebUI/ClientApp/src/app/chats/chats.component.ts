import { Component, OnInit } from '@angular/core';
import { ChatMessageDto, ChatMessageStatus, ChatRoomDto, ChatRoomsClient, ProductDto, ProductsClient, ChatUserDto, ReadMessageCommand } from '../rentasgt-api';
import { PageInfo } from '../models/PageInfo';
import { AuthorizeService, IUser } from '../../api-authorization/authorize.service';
import { ActivatedRoute } from '@angular/router';
import * as signalR from '@microsoft/signalr';
import { DateTime } from 'luxon';

@Component({
  selector: 'app-chats',
  templateUrl: './chats.component.html',
  styleUrls: ['./chats.component.css']
})
export class ChatsComponent implements OnInit {

  public ROOMS_PAGE_SIZE = 30;
  public ROOMS_DEFAULT_PAGE_NUMBER = 1;
  public MSG_PAGE_SIZE = 45;
  public MSG_DEFAULT_PAGE_NUMBER = 1
  public chats: ChatRoomDto[] = [];
  public pageInfo: PageInfo = null;
  public loadingChatRooms = false;
  public selectedChatRoom: ChatRoomDto = null;
  public product: ProductDto = null;
  public pageInfoMsg: PageInfo = null;
  public loadingMessages = false;
  public messages: ChatMessageDto[] = [];
  public currentUser: IUser = null;
  public newMessage: string = '';
  public sendingMessage = false;

  public MSG_STATUS_LABELS = [];
  public MSG_STATUS_NOTREAD = ChatMessageStatus.SinLeer;

  private hubConnection: signalR.HubConnection = null;

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
    private chatRoomsClient: ChatRoomsClient,
    private productsClient: ProductsClient,
    private authorizeService: AuthorizeService,
    private activatedRoute: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    this.MSG_STATUS_LABELS[ChatMessageStatus.SinEnviar] = 'No enviado';
    this.MSG_STATUS_LABELS[ChatMessageStatus.SinLeer] = 'Enviado';
    this.MSG_STATUS_LABELS[ChatMessageStatus.Leido] = 'Visto';
    this.loadChatRooms();
    this.activatedRoute.paramMap.subscribe(params => {
      const roomId = Number.parseInt(params.get('roomId'));
      const msg = params.get('msg');
      if (roomId) {

        this.chatRoomsClient.getById(roomId)
          .subscribe(async (res) => {
            this.selectChatRoom(res);
            if (msg && msg.trim().length > 0) {
              this.newMessage = msg;
              await this.sendMessage();
            }
          }, console.error);
      }
    });
    this.authorizeService.getUser().subscribe((res) => {
      this.currentUser = res;
    }, console.error);
    this.connect();

  }

  public loadChatRooms(pageSize = this.ROOMS_PAGE_SIZE, pageNumber = this.ROOMS_DEFAULT_PAGE_NUMBER) {
    this.loadingChatRooms = true;
    this.chatRoomsClient.get(pageSize, pageNumber)
      .subscribe((res) => {
        this.chats = res.items;
        this.pageInfo = new PageInfo(res.currentPage, res.totalPages, res.pageSize, res.totalCount);
        this.loadingChatRooms = false;
      }, error => {
        console.error(error);
        this.loadingChatRooms = false;
      });
  }

  public connect(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/chathub', {
        accessTokenFactory: async () => `${await this.authorizeService.getAccessToken().toPromise()}`
      })
      .build();
    this.hubConnection.on('receiveMessage', (message: ChatMessageDto) => {
      this.receiveMessage(message);
    });
    this.hubConnection.on('messageReadNotification', (message: ChatMessageDto) => {
      this.messageReadNotification(message);
    });
    this.hubConnection
      .start()
      .then(() => console.log('Connected to chathub'))
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  public receiveMessage(message: ChatMessageDto): void {
    const chatIndex = this.chats.findIndex(chat => chat.id === message.roomId);
    if (chatIndex >= 0) {
      message.sentDate = new Date(message.sentDate);
      const chat = this.chats.splice(chatIndex, 1)[0];
      chat.lastMessage = message;
      this.chats.unshift(chat);
    }
    if (this.selectedChatRoom && this.selectedChatRoom.id == message.roomId) {
      this.messages.push(message);
      setTimeout(this.scrollToBottomChat, 350);
    }
  }

  public messageReadNotification(message: ChatMessageDto): void {
    message.sentDate = new Date(message.sentDate);
    if (this.selectedChatRoom && this.selectedChatRoom.id == message.roomId) {
      const msgIndex = this.messages.findIndex(msg => msg.id === message.id);
      if (msgIndex >= 0) {
        let i = msgIndex;
        while (i >= 0) {
          const currMsg = this.messages[i];
          if (currMsg.status == ChatMessageStatus.SinLeer
            && currMsg.sender.id == message.sender.id) {
            this.messages[msgIndex].status = ChatMessageStatus.Leido;
          }
          --i;
        }


      }
      if (this.selectedChatRoom.lastMessage.id === message.id) {
        this.selectedChatRoom.lastMessage.status = message.status;
      }
    } else {
      const chatIndex = this.chats.findIndex(chat => chat.lastMessage.id === message.id);
      if (chatIndex >= 0) {
        this.chats[chatIndex].lastMessage.status = message.status;
      }
    }

  }

  public async sendMessage(): Promise<any> {
    this.sendingMessage = true;
    if (this.hubConnection !== null) {
      const msg: ChatMessageDto = await this.hubConnection.invoke('SendMessage', this.selectedChatRoom.id, this.newMessage);
      if (msg !== null) {
        msg.sentDate = new Date(msg.sentDate);
        this.messages.push(msg);
        setTimeout(this.scrollToBottomChat, 350);
      }
      this.newMessage = '';
    }

    this.sendingMessage = false;
  }

  public isValidNewMessage(): boolean {
    return this.newMessage.trim().length > 0;
  }

  public selectChatRoom(chatRoom: ChatRoomDto): void {
    this.product = null;
    this.selectedChatRoom = chatRoom;
    this.loadMessages(this.selectedChatRoom.id);
    this.loadProduct(this.selectedChatRoom.product.id);
  }

  private loadProduct(idProduct: number): void {
    this.productsClient.getById(idProduct)
      .subscribe((res: ProductDto) => {
        this.product = res;
      }, console.error);
  }

  public deselectChatRoom(): void {
    this.selectedChatRoom = null;
    this.loadChatRooms();
  }

  public isFromOther(msg: ChatMessageDto): boolean {
    return this.currentUser !== null && msg.sender.id !== this.currentUser.sub;
  };

  public formatDate(date: Date): string {
    return DateTime.fromJSDate(date).toFormat('dd/MM/yyyy t');
  }

  private loadMessages(roomId: number, pageSize = this.MSG_PAGE_SIZE,
    pageNumber = this.MSG_DEFAULT_PAGE_NUMBER): void {
    this.loadingMessages = true;
    this.chatRoomsClient.getMessages(roomId, pageSize, pageNumber)
      .subscribe((res) => {
        this.pageInfoMsg = new PageInfo(res.currentPage, res.totalPages, res.pageSize, res.totalCount);
        this.messages = res.items;
        setTimeout(() => {
          this.scrollToBottomChat();
          if (this.messages.length > 0) {
            const lastMessage = this.messages[this.messages.length - 1];
            if (lastMessage.sender.id !== this.currentUser.sub
              && lastMessage.status == ChatMessageStatus.SinLeer) {
              const readMessageCommand = new ReadMessageCommand();
              readMessageCommand.messageId = lastMessage.id;
              this.chatRoomsClient.readMessage(readMessageCommand)
                .subscribe((msg: ChatMessageDto) => {
                  this.messageReadNotification(msg);
                }, console.error);
            }
          }
        }, 800);
        this.loadingMessages = false;
      }, error => {
        console.error(error);
        this.loadingMessages = false;
      });
  }

  public getOtherUserName(): string {
    if (!this.selectedChatRoom || !this.currentUser) return 'Unknown';
    let user: ChatUserDto = this.selectedChatRoom.user;
    if (user.id === this.currentUser.sub) {
      user = this.selectedChatRoom.product.owner;
    }
    return user.firstName + ' ' + user.lastName;
  }

  private scrollToBottomChat(): void {
    const scroll = document.querySelector('#chat-scroll');
    scroll.scrollTo({
      top: scroll.scrollHeight,
      left: 0,
      behavior: 'smooth'
    });
  }

}
