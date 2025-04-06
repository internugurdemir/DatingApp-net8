import { Component, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private accountService = inject(AccountService);
  // private toastr = inject(ToastrService);
  // constructor(private accountService: AccountService, private toastr: ToastrService, 
  //   private fb: FormBuilder, private router: Router) { }
  
  
  cancelRegister = output<boolean>();
  // // old way of output 
  // @Output() cancelRegister = new EventEmitter();
  

  // @Input() usersFromHomeComponent:any;
  // usersFromHomeComponent = input.required<any>();

    model: any = {}
  
    register(){
      this.accountService.register(this.model).subscribe({//for observable we used subscribe
        next: response=>{
          console.log(response);
          this.cancel();
        },
        error: error=> console.error(error.error)
        // error: error=> this.toastr.error(error.error)
      })
    }
    cancel(){
      this.cancelRegister.emit(false);
      // console.log('cancelled');
    }
}
