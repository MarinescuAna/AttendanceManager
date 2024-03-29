import { extend } from "vee-validate";
import { required, email,  between, min, length } from "vee-validate/dist/rules";

extend("required", required);
extend("email", email);
extend("between", between);
extend("min", min);
extend("length", length);


export const rules = {
    required: {
        required: true
    },
    between_0_30: {
        required: true,
        between: [0,30]
    },
    length_8: {
        required: true,
        length: [8]
    },
    between_0_100: {
        required: true,
        between: [0,100]
    },
    email: {
        required: true,
        email: true
    },
    password:{
        required:true,
        min: [6]
    }
};
