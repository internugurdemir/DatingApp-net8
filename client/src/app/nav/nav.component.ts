import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule,BsDropdownModule ],
  // imports: [FormsModule, BsDropdownModule, RouterLink,RouterLinkActive, TitleCasePipe],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  accountService = inject(AccountService);
  // private router = inject(Router)
  // private toastr = inject(ToastrService);
  model: any = {}

  // constructor(public accountService: AccountService, private router: Router, 
  //   private toastr: ToastrService) { }

  // ngOnInit(): void {
  // }

  login() {
    // console.log(this.model);
    this.accountService.login(this.model).subscribe(
      { 
        next: response => {
          console.log(response);
        },

        error: error => console.log(error)

        // next: _ => { this.router.navigateByUrl('/members') },

        // error: error => this.toastr.error(error.error)
      }
    )
  }

  logout() {
    this.accountService.logout();
    // // this.accountService.logout();
    // this.router.navigateByUrl('/');
  }
}
