import { extend } from "vee-validate";
import { required, email,  between, min } from "vee-validate/dist/rules";

extend("required", required);
extend("email", email);
extend("between", between);
extend("min", min);


export const rules = {
    required: {
        required: true
    },
    between_0_30: {
        required: true,
        between: [0,30]
    },
    email: {
        required: true,
        email: true
    },
    password:{
        required:true,
        min: [6]
    },
    between_1_240:{
        required:true,
        between: [1,240]
    }
};
