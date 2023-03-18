/**
 * Plugin documentation:
 * https://github.com/Maronato/vue-toastification/tree/main
 * https://vue-toastification.maronato.dev/
 */

import Toast from "vue-toastification";
import "vue-toastification/dist/index.css";
import Vue from 'vue'
import ToastificationComponent from '@/components/shared-components/ToastificationComponent.vue'

/**
 * Configuration the toast popup 
 */
const options = {
  position: "top-right",
  timeout: 5000,
  closeOnClick: true,
  pauseOnFocusLoss: true,
  pauseOnHover: true,
  draggable: true,
  draggablePercent: 0.6,
  showCloseButtonOnHover: false,
  hideProgressBar: true,
  closeButton: false,
  icon: true,
  rtl: false
};

Vue.use(Toast, options);

/**
 * This is a service for handling the messages
 */
export class Toastification {
  /**
   * This is a simple method for displaying an error without a title
   * @param message error 
   */
  static simpleError(message: string): void {
    Vue.$toast.error({
      component: ToastificationComponent,
      props: {
        description: message
      },
    });
  }

  /**
   * This is a simple method for displaying an error with a title
   * @param message error
   * @param title in most of the cases this will be the status and the type of the error: 400 Not found
   */
  static error(message: string, title: string): void {
    Vue.$toast.error({
      component: ToastificationComponent,
      props: {
        description: message,
        title: title
      },
    });
  }

  /**
   * This is a simple method for displaying a warning
   * @param message  
   */
  static warning(message: string): void {
    Vue.$toast.warning({
      component: ToastificationComponent,
      props: {
        description: message
      },
    });
  }

  /**
   * This is a simple method for displaying an information
   * @param message  
   */
  static info(message: string): void {
    Vue.$toast.info({
      component: ToastificationComponent,
      props: {
        description: message
      },
    });
  }

  /**
   * This is a simple method for displaying a success message
   * @param message  
   */
  static success(message: string): void {
    Vue.$toast.success({
      component: ToastificationComponent,
      props: {
        description: message
      },
    });
  }

}