import { Component, inject, OnInit } from '@angular/core';
// import { RegisterComponent } from "../register/register.component";
import { HttpClient } from '@angular/common/http';
import { RegisterComponent } from "../register/register.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css' 
})
export class HomeComponent implements OnInit{
  http = inject(HttpClient);  
  registerMode = false;
  users:any;


  ngOnInit(): void {
    this.getUsers(); 
    // throw new Error('Method not implemented.');
  }
  
  cancelRegisterMode(event:boolean){
    this.registerMode= event;
  }

  
  registerToggle(){
    this.registerMode = !this.registerMode
  }

  
  getUsers(){
    this.http.get('https://localhost:5001/api/users').subscribe({
      // next:()=> {},
      next: response => this.users = response,
      error: error   => console.log(error),
      complete:()=> console.log('Request has completed')
    });
  }
}
