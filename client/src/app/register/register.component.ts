import { Component, inject, input, output} from '@angular/core';
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
// constructor(private accountService: AccountService, private toastr: ToastrService, 
//   private fb: FormBuilder, private router: Router) { }
cancelRegister = output<boolean>();
// old way of upper 
//@Output() cancelRegister = new EventEmitter();


  model: any = {}

  register(){
    this.accountService.register(this.model).subscribe({
      next: response=>{
        console.log(response);
        this.cancel();
      },
      error: error=> console.log(error)
    })
  }
  cancel(){
    this.cancelRegister.emit(false);
    console.log('cancelled');
  }
}
