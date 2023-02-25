export class ImageSelector{
    static selectClockImage(index: number): string {
        if(index%11==0){
            return "clocks/clock11.jpg";
        }else if(index%7==0){
            return "clocks/clock7.jpg";
        }else if(index%3==0){
            return "clocks/clock3.jpg"; 
        }else{
            return "clocks/clock2.jpg";
        } 
    }
}