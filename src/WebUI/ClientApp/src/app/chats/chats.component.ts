import { Component, OnInit } from '@angular/core';
import { ChatMessageDto, ChatMessageStatus, ChatRoomDto, ChatRoomsClient } from '../rentasgt-api';
import { PageInfo } from '../models/PageInfo';
import { AuthorizeService, IUser } from '../../api-authorization/authorize.service';
import { ActivatedRoute } from '@angular/router';
import * as signalR from '@microsoft/signalr';

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
  public pageInfoMsg: PageInfo = null;
  public loadingMessages = false;
  public messages: ChatMessageDto[] = [];
  public currentUser: IUser = null;
  public sendingMessage = false;

  public MSG_STATUS_LABELS = [];

  private hubConnection: signalR.HubConnection = null;

  constructor(
    private chatRoomsClient: ChatRoomsClient,
    private authorizeService: AuthorizeService,
    private activatedRoute: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    this.MSG_STATUS_LABELS[ChatMessageStatus.SinEnviar] = 'No enviado';
    this.MSG_STATUS_LABELS[ChatMessageStatus.SinLeer] = 'Enviado';
    this.MSG_STATUS_LABELS[ChatMessageStatus.Leido] = 'Visto';
    this.loadChatRooms();
    this.activatedRoute.paramMap.subscribe(params => {
      const id = Number.parseInt(params.get('room'));
      if (id) {
        while (this.loadingChatRooms === true) {}
        // TODO: load
      }
    });
    this.authorizeService.getUser().subscribe((res) => {
      this.currentUser = res;
    }, console.error);
    
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
      this.hubConnection.on('receiveMessage', this.receiveMessage);
    this.hubConnection
      .start()
      .then(() => console.log('Connected to chathub'))
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  public receiveMessage(message: ChatMessageDto): void {
    const chatIndex = this.chats.findIndex(chat => chat.id === message.roomId);
    if (chatIndex >= 0)
    {
      const chat = this.chats.splice(chatIndex, 1)[0];
      chat.lastMessage = message;
      this.chats.unshift(chat);
    }
  }

  public async sendMessage(content: string): Promise<any> {
    this.sendingMessage = true;
    if (this.hubConnection !== null)
    {
      const msg: ChatMessageDto = await this.hubConnection.invoke('SendMessage', this.selectedChatRoom.id, content);
      if (msg !== null)
      {
        this.messages.push(msg);
      }
    }
    this.sendingMessage = false;
  }

  public selectChatRoom(chatRoom: ChatRoomDto): void {
    if (!this.selectedChatRoom || chatRoom.id !== this.selectedChatRoom.id) {
      this.selectedChatRoom = chatRoom;
      this.loadMessages(this.selectedChatRoom.id);
    }
  }

  private loadMessages(roomId: number, pageSize = this.MSG_PAGE_SIZE,
                       pageNumber = this.MSG_DEFAULT_PAGE_NUMBER ): void {
    this.loadingMessages = true;
    this.chatRoomsClient.getMessages(roomId, pageSize, pageNumber)
      .subscribe((res) => {
        this.pageInfoMsg = new PageInfo(res.currentPage, res.totalPages, res.pageSize, res.totalCount);
        this.messages = res.items;
        this.loadingMessages = false;
      }, error => {
        console.error(error);
        this.loadingMessages = false;
      });
  }

}
