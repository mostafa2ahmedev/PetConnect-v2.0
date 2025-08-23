import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from './Feature/header/header';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Footer } from './Feature/footer/footer';
@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Header, NgbModule, Footer],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected title = 'PetConnect';
}
