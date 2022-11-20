
 export default class ResponseHandler{
  public static errorResponseHandler(error: Response|any):boolean {
    console.log(error);
    alert("API doesn't respond!")
     return false;
 }
}


export interface Response{
    error: string;
    status: number;
}
 