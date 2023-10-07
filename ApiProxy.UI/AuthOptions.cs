﻿using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiProxy
{
    public static class AuthOptions
    {
        public const string ISSUER = "ApiProxy";
        public const string AUDIENCE = "Merchant";
        public static SymmetricSecurityKey KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("FXiwYSr@798i8I#PpiP&Ayqk3vWn2gllp3ZT*^!toD07c=x$mV0hb5=$l1Ty(%h^.0g^nyXw6yWGeStwxSKREZ@CnIQ4V#r0^&x(cICM3ed*If3QD6z1XC@ghMRQWmsS3BSQO6n2VJ1%)oP*t(A^8mA^evCTJsFuC8bKs!)n@EdiDGkvh4F=x9$7oXn(vCHf.dm.HR$RVhiUeWqPkUatC*.%0=Ak#x%x3B%lUXg*6=%awmAjhDA%F#X6yR$OVlgOxqidFKoLs6*Ljk3zYBtm.ILq$ov2bTC0joqw1WJ=IznLvvns@irD=igQh#iDj&0iIsxf6nt-qkv4iK%$Oeu2H@(qKd7QLUMQdpSVSOgbjHqe.4)3A2RSvacV@oUd$NQ#0aJsvUMv5ZNK%fpBaPl7WXP.GFPRdHgHimlZi1J7RGsYsMFj8(71O4D8IoETeqy.pbu0x@v8f2tSRr=yFZB$ZcCrb%Dy-9)24)JhKQYqzm7A1SCMijJ=WVo)D3E$7G(%az2%@9i8@^(uIx8sD^ZprU&pGAcciz-P45fpm5D9=R.w8B6JBWEi6iBGqY4ECYn-DX^pm)cfGoAdrJ*NNXJaXY7iSTtqY*U!#QtS1FDoB@!WoM.7p3q4Fuw7dJrP@*mLPf5lll57pptt8k6135sWP%D8L6Q0xdXRu^Nt2v$pX03OySvP5KZZTo189MK1.f#=ZfsFGAUW-RcQ#nA2rxli-C(-i09M#XVHPU#PUfS!^wX$*iK2!Wbkc&71P^U6td%vZyCLGPQ78hd)3G2@sO5Q=ap.)sdgb1wGT#OD=HdemRPu.yYbs%qQ$TgaTqs4H-$7FxBX=hNcIe&(JV4W7UH2V.hGE##B@iV(oP$nID!EtNA&9oC)Y2e9qteN6F-yZAZWo8.qz4XT^eE.@duQZ*ZpEmhTTBR76Ajg^m8w#P7Q!^)q(ihXQX)QIxn4835%59hsqYXKTc0jQ%^b#QCC)ftiMqxYlbem%1vy1EmU1g-!%w3aZBANh%#Bb-3s(ni8Ft%Lno8&Tk&MZsSYBW&v0o9RvGWnyi1Ql@QMXtZrOHtq7jN*R)kgWgpA%!@XwNlEF9N0.xC0pNvnCb(BNA@c^)LolwUAip3y*txZbfS!lLdcveX7hUObt&KWLV5!&z)6)=jKS*v*uZ7FCiAe#9XTt.hzwz067.!neL(XE4*jeN#@dK$)gAhUCeo8Nb^0RChN74EJmp&&-dLjs1uC*cQolI!yoJ#hGPBUo#4tEkoEktyH-=LBP-%wJRiLd$v(Rx1bD1BPhBpgJolmgeVqPQu)quol3tQ0m1Eujt2-Kvqu.o)z4u6Q!sPtn(sOqcxjgwwRwl$1o#(rKR%o43bokBb@f$AC*q7mYJb5Hr^DtW41ehah5(6z.d9K5ZJM&H2KA1KZ)VN*X8kAGyXMUtn7V2eGzy$PwkiNjU43yg!#FFy4d.iUoDJU=-Q%yDkJwaaHgL^.!jAOk^uEdGXZB#z3HzCdf#zhC3KuMOO8@^8j#1PiQsGhylJ&n*AMbI*7DTmarh4c!V.jl9j$C9tz8yt$w.ck19Gdhj=xj.TwN0SOh1D#jXxj7MZEN24YpJHZoyzpCLzLEO0kq48pE5TFvc7*3&YBLBbCR9L9EtcO^cv4c$LeVo*L&Q0%ikhrbNBVr(eR05rwmo6Yb#miiI(&1IToD!Fj.ld**M3L1c=71$u^r5s=AiW2H@63DG!L.tQorc9D36iEEF5ExC$RAz7PivtnNKbB9(D(6xUT1A*-#&hYeJf-GeyMv8b43XJZlJ*0QE3yKVZn%eBEft=%NbyDkqo)48Y&dW9PxCc)@dxE6nzwG02.NY1hJcefT8d*(*Q4JPTZqRYphWy7l93OtSqsAZPo3==cQRo3t^^Y!KNnF&(wD06d.KNmxrp*cpGOu1qtJZY5TH01Dwtl#%t8hs&CKLXw$.dlZGT@6U^rc&KHdWqXqL8P25&ssSspuMRlc-(ufUeAWo*uMVfI2ovCl-**DTFoIgcdfZt^GSLnoTg0g7jHQ0gEJitpObK8BcTogz#qgjQlX^HY#mmS"));
    }
}
