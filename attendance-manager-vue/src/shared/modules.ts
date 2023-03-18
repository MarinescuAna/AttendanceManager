
/**
 * Used to filter the response from the API and send the response to components
 */
export interface ResponseModule{
    error: string;
    isSuccess: boolean;
}
/**
 * Token date after decoding
 */
 export interface TokenData{
    email:string;
    role: string;
    name: string;
    code: string;
}










