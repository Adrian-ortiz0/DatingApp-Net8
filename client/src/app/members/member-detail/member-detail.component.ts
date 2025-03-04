import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { ActivatedRoute } from '@angular/router';
import { Member } from '../../_models/member';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit {
  private memberService = inject(MembersService);
  private route = inject(ActivatedRoute);
  member?: Member;

  ngOnInit(): void {
    this.loadMember();
  }
  loadMember(){
    const name = this.route.snapshot.paramMap.get("name");
    if(!name){
      return;
    }
    this.memberService.getMember(name).subscribe({
      next: member => this.member = member
    })
  }
}
