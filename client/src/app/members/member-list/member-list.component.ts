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
  memberService = inject(MembersService);
  genderList = [{value: 'male', display: 'Males'}, {value: 'female', display: 'Females'}]

  ngOnInit(): void {
    if (this.memberService.members().length === 0) this.loadMembers();
  }

  loadMembers() {
    this.memberService.getMembers()
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
