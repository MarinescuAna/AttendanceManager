<template>
  <v-card class="pa-4">
    <validation-observer v-slot="{ handleSubmit, invalid }">
      <v-form @submit.prevent="handleSubmit(onSubmit)">
        <v-layout column class="pa-2">
          <v-card-title>
            <p>Generate attendance code</p>
            <v-spacer></v-spacer>
            <v-btn icon @click="onClose">
              <v-icon>mdi-close</v-icon>
            </v-btn>
          </v-card-title>
          <v-card-text>
            <v-flex>
              <validation-provider
                name="expiration time (minutes)"
                v-slot="{ errors }"
                :rules="rules.between_1_240"
              >
                <v-text-field
                  v-model="time"
                  label="Expiration time (minutes between 1-240)"
                  type="number"
                  :error-messages="errors"
                  prepend-icon="mdi-clock-time-four-outline"
                ></v-text-field>
              </validation-provider>
            </v-flex>
            <v-flex v-if="generatedCode !== ''">
              <p v-html="generatedCode" class="pa-2"></p>
            </v-flex>
          </v-card-text>
          <v-card-actions>
            <v-layout class="pa-2" justify-center align-content-end row>
              <v-btn
                color="black"
                class="mr-1 white--text"
                :disabled="invalid"
                @click="onSubmit"
              >
                Generate code
              </v-btn>
            </v-layout>
          </v-card-actions>
        </v-layout>
      </v-form>
    </validation-observer>
  </v-card>
</template>

<script lang="ts">
import AttendanceCodeService from "@/services/attendance-code.service";
import { rules } from "@/plugins/vee-validate";
import Vue from "vue";

export default Vue.extend({
  data() {
    return {
      rules,
      time: 0,
      generatedCode: "",
    };
  },
  methods: {
    onClose: function (): void {
      this.$emit("close");
    },
    onSubmit: async function (): Promise<void> {
      const result = await AttendanceCodeService.createAttendanceCode(
        this.time
      );

      if (typeof result !== "undefined") {
        this.generatedCode = `<b><h3>${result.code}</h3></b> (available until ${result.expirationDate})`;
      }
    },
  },
});
</script>