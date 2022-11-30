
<template>
  <v-container>
    <v-row justify="center">
      <v-card width="50%" class="orange lighten-3">
        <v-card-title class="pa-7">
          <h2>Sign in</h2>
        </v-card-title>
        <v-card-text>
          <validation-observer v-slot="{ handleSubmit, invalid }" >
            <v-form @submit.prevent="handleSubmit(doLogin)">
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
                name="password"
                v-slot="{ errors }"
                rules="required|min:4"
              >
                <v-text-field
                  v-model="password"
                  :type="showPassword ? 'text' : 'password'"
                  label="Password"
                  prepend-icon="mdi-lock"
                  :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                  @click:append="showPassword = !showPassword"
                  :error-messages="errors"
                  required
                  class="pa-6"
                />
              </validation-provider>
              <v-row justify="center"  class="pa-8">
              <v-btn width="50%" @click="doLogin" :disabled="invalid" large>Sign in</v-btn>
            </v-row>
            </v-form>
          </validation-observer>
        </v-card-text>
      </v-card></v-row>
  </v-container>
</template>

<script lang="ts">
import Vue from "vue";
import AuthService from "@/services/auth.service";
import { extend } from "vee-validate";
import { required, email, min } from "vee-validate/dist/rules";
import { EventBus } from "@/main";
import { EVENT_BUS_ISLOGGED } from "@/shared/constants";

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
      // Boolean for hidding or displaying the password
      showPassword: false,
      // Password
      password: "",
      // Email
      email: "",
    };
  },
  methods: {
    /**
     * Use this method for login: if the login is done, update the navbar and redirect to home page
     */
    async doLogin() {
      const response = await AuthService.login({
        email: this.email,
        password: this.password,
      });

      if (response.isSuccess) {
        EventBus.$emit(EVENT_BUS_ISLOGGED);
        this.$router.push("/");
      }else{
        window.alert(response.error);
      }
    },
  },
});
</script>
  