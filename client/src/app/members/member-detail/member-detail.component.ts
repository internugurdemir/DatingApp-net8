import { Component, inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Member } from '../../_models/member';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { MemberMessagesComponent } from "../member-messages/member-messages.component";
import { MessageService } from '../../_services/message.service';
import { PresenceService } from '../../_services/presence.service';
import { AccountService } from '../../_services/account.service';
import { HubConnection, HubConnectionState } from '@microsoft/signalr';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [TabsModule, GalleryModule, TimeagoModule, DatePipe, MemberMessagesComponent],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs', {static:true}) memberTabs?: TabsetComponent;
  private messageService = inject(MessageService);
  private accountService = inject(AccountService);
  presenceService = inject(PresenceService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  member: Member = {} as Member;
  images: GalleryItem[] = [];
  activeTab?: TabDirective;

  ngOnInit(): void {
    this.route.data.subscribe({
      next: data => {
        this.member = data['member'];
        this.member && this.member.photos.map(p => {
          this.images.push(new ImageItem({ src: p.url, thumb: p.url }))
        })
      }
    });

    this.route.paramMap.subscribe({
      next : _=> this.OnRouteParamsChange()
    })

    this.route.queryParams.subscribe({
      next: params => {
        params['tab'] && this.selectTab(params['tab'])
      }
    })
  }

  selectTab(heading: string) {

    if (this.memberTabs) {
      const messageTab = this.memberTabs.tabs.find(a => a.heading === heading);
      if (messageTab) messageTab.active = true;
    }
  }
  OnRouteParamsChange() {
    const user =this.accountService.currentUser();
    if (!user) return;
    if (this.messageService.hubConnection?.state===HubConnectionState.Connected
          && this.activeTab?.heading === 'Messages')
    {
      this.messageService.hubConnection.stop().then(()=>{
        this.messageService.createHubConnection(user, this.member.username)
      })
    }
  }
  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    this.router.navigate([], {
      relativeTo:this.route,
      queryParams: {tab:this.activeTab.heading},
      queryParamsHandling:'merge'
    })
    if (this.activeTab.heading === 'Messages' && this.member) {
      const user = this.accountService.currentUser();
      if (!user) return;
      this.messageService.createHubConnection(user,this.member.username);
    }else{
      this.messageService.stopHubConnection();
    }
  }
  //   loadMember(){
  //     // What does this.route.snapshot.paramMap.get('username') do?
  //     // this.route: Gives access to the current route.
  //     // .snapshot: Takes a "snapshot" (a static copy) of the current route at the moment.

  //     // .paramMap.get('username'): Gets the value of the username parameter from the URL.

  //     const username = this.route.snapshot.paramMap.get('username');

  //     if (!username) return;

  //     this.memberService.getMember(username).subscribe({
  //         next: member => {
  //           this.member=member;
  //           member.photos.map(p=>{
  //             this.images.push(new ImageItem({src: p.url, thumb: p.url}))
  //           })
  //         }
  //       })

  //   }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

}
