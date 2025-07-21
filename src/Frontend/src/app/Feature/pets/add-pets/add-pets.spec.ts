import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddPets } from './add-pets';

describe('AddPets', () => {
  let component: AddPets;
  let fixture: ComponentFixture<AddPets>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddPets]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddPets);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
