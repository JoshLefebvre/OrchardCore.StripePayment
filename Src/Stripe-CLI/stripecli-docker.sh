docker run --rm -it stripe/stripe-cli --api-key sk_test_XqMhoNDFBzqKaTWTQssYWpFj listen --forward-to=listen --forward-to https://localhost:5001/api/stripewebhook/sync