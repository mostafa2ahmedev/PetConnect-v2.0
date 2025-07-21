import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdatePet } from './update-pet';

describe('UpdatePet', () => {
  let component: UpdatePet;
  let fixture: ComponentFixture<UpdatePet>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdatePet]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdatePet);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
