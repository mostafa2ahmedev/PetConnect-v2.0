import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {
  doctors = [{ fName: 'h', id: 1, lName: 'e', petSpecialty: 'cat' }];
}
