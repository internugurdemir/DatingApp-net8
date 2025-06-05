import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/member';
import { MemberCardComponent } from '../member-card/member-card.component';

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {
  private memberService = inject(MembersService);
  members:Member[]=[];
  genderList = [{value: 'male', display: 'Males'}, {value: 'female', display: 'Females'}]

  ngOnInit(): void {
     this.loadMembers();
    // if (!this.memberService.paginatedResult()) this.loadMembers();
  }

  loadMembers() {
    this.memberService.getMembers().subscribe({ next: members => this.members = members})
  }

  // resetFilters() {
  //   this.memberService.resetUserParams();
  //   this.loadMembers();
  // }

  // pageChanged(event: any) {
  //   if (this.memberService.userParams().pageNumber != event.page) {
  //     this.memberService.userParams().pageNumber = event.page;
  //     this.loadMembers();
  //   }
  // }
}
