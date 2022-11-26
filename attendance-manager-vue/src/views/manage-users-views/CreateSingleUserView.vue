
<template>
  <v-container>
    <v-row justify="center">
      <v-card width="50%" class="orange lighten-3">
        <v-card-title class="pa-7">
          <h2>Create single user</h2>
        </v-card-title>
        <v-card-text>
          <validation-observer v-slot="{ handleSubmit, invalid }">
            <v-form @submit.prevent="handleSubmit(onSubmit)">
              <validation-provider
                rules="required"
                name="fullname"
                v-slot="{ errors }"
              >
                <v-text-field
                  label="Fullname"
                  v-model="fullname"
                  prepend-icon="mdi-account"
                  :error-messages="errors"
                  class="pa-6"
                  required
                />
              </validation-provider>
              <validation-provider
                rules="required|email"
                name="email"
                v-slot="{ errors }"
              >
                <v-text-field
                  label="Email"
                  v-model="email"
                  prepend-icon="mdi-email"
                  :error-messages="errors"
                  class="pa-6"
                  required
                />
              </validation-provider>
              <validation-provider
                rules="required"
                name="GDPR code"
                v-slot="{ errors }"
              >
                <v-text-field
                  label="GDPR Code"
                  v-model="code"
                  prepend-icon="mdi-account"
                  :error-messages="errors"
                  class="pa-6"
                  required
                />
              </validation-provider>
              <v-container class="ml-3">
                <label class=""><v-icon>mdi-pencil</v-icon> Role</label>
                <v-radio-group v-model="role" class="ml-4">
                <v-radio label="Student" :value="1"></v-radio>
                <v-radio label="Teacher" :value="2"></v-radio>
              </v-radio-group>
              </v-container>
              <v-row justify="center" class="pa-8">
                <v-btn width="50%" @click="onSubmit" :disabled="invalid" large
                  >Submit</v-btn
                >
              </v-row>
            </v-form>
          </validation-observer>
        </v-card-text>
      </v-card></v-row
    >
  </v-container>
</template>
  
  <script lang="ts">
import Vue from "vue";
import UserService from "@/services/user.service";
import { extend } from "vee-validate";
import { required, email, min } from "vee-validate/dist/rules";
import { Role } from "@/shared/enums";
import { CreateUserParameters } from "@/shared/modules";

/**
 * Validation for requied
 */
extend("required", {
  ...required,
  message: "{_field_} can not be empty",
});

/**
 * Validation for minimum length
 */
extend("min", {
  ...min,
  message: "{_field_} must be greater than {length} characters",
});

/**
 * Validation for email
 */
extend("email", {
  ...email,
  message: "Email must be valid",
});

export default Vue.extend({
  data() {
    return {
      // Email
      email: "",
      //Fullname
      fullname: "",
      //GDPR code
      code: "",
      //user's role; Default value is 1 => student
      role: 1,
    };
  },
  methods: {
    /**
     * Use this method for login: if the login is done, update the navbar and redirect to home page
     */
    async onSubmit(): Promise<void> {
      const response = await UserService.CreateUser({
        fullname: this.fullname,
        code: this.code,
        email: this.email,
        role: this.role == 1 ? Role.Student : Role.Teacher,
      } as CreateUserParameters);

      //   if (response) {
      //     EventBus.$emit(EVENT_BUS_ISLOGGED);
      //     this.$router.push("/");
      //   }
    },
  },
});
</script>
    